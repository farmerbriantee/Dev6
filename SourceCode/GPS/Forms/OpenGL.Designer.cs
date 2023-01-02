using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        private const float fovy = 0.7f, camDistanceFactor = 4.0f;

        private int mouseX = 0, mouseY = 0;
        private bool updateZoomWindow = false;
        private int steerModuleConnectedCounter = 0;

        //data buffer for pixels read from off screen buffer
        private byte[] grnPixels = new byte[150001];
        private int bbCounter = 0, deadCam = 0;
        public double maxFieldX, maxFieldY, minFieldX, minFieldY;
        public double eastMin, eastMax, northMin, northMax;

        public vec2 screenLeft, screenRight, screenTop, screenBottom;

        public double fieldCenterX, fieldCenterY, maxFieldDistance, maxCrossFieldLength, avgPivDistance, avgPivDistanceTool;
        public Matrix4 inv_view, inv_proj;


        // When oglMain is created
        private void oglMain_Load(object sender, EventArgs e)
        {
            oglMain.MakeCurrent();
            LoadGLTextures();
            GL.ClearColor(0.27f, 0.4f, 0.7f, 1.0f);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.CullFace(CullFaceMode.Back);
            GL.EnableClientState(ArrayCap.VertexArray);
        }

        //oglMain needs a resize
        private void oglMain_Resize(object sender, EventArgs e)
        {
            oglMain.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Viewport(0, 0, oglMain.Width, oglMain.Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(fovy, oglMain.AspectRatio, 1.0f, (float)(camDistanceFactor * worldManager.camSetDistance));
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void oglMain_Paint(object sender, PaintEventArgs e)
        {
            if (sentenceCounter > 299)
            {

                //sentenceCounter = 0;
                GL.Enable(EnableCap.Blend);
                GL.ClearColor(0.122f, 0.1258f, 0.1275f, 1.0f);

                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                GL.LoadIdentity();

                //match grid to cam distance and redo perspective
                oglMain.MakeCurrent();

                GL.Translate(0.0, 0.3, -10);
                //rotate the camera down to look at fix
                GL.Rotate(deadCam, 0.0, 1.0, 0.0);

                deadCam += 5;

                GL.Enable(EnableCap.Texture2D);
                GL.Color4(1.25f, 1.25f, 1.275f, 0.75);
                GL.BindTexture(TextureTarget.Texture2D, texture[21]);        // Select Our Texture
                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                GL.TexCoord2(1, 0); GL.Vertex2(2.5, 2.5); // Top Right
                GL.TexCoord2(0, 0); GL.Vertex2(-2.5, 2.5); // Top Left
                GL.TexCoord2(1, 1); GL.Vertex2(2.5, -2.5); // Bottom Right
                GL.TexCoord2(0, 1); GL.Vertex2(-2.5, -2.5); // Bottom Left
                GL.End();                       // Done Building Triangle Strip

                GL.Disable(EnableCap.Texture2D);

                // 2D Ortho ---------------------------------------////////-------------------------------------------------

                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.LoadIdentity();

                //negative and positive on width, 0 at top to bottom ortho view
                GL.Ortho(-(double)oglMain.Width / 2, (double)oglMain.Width / 2, (double)oglMain.Height, 0, -1, 1);


                //  Create the appropriate modelview matrix.
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadIdentity();

                GL.Color3(0.98f, 0.98f, 0.70f);

                int edge = -oglMain.Width / 2 + 10;

                font.DrawText(edge, oglMain.Height - 240, "<-- AgIO ?");

                GL.Flush();//finish openGL commands
                GL.PopMatrix();//  Pop the modelview.

                ////-------------------------------------------------ORTHO END---------------------------------------

                //  back to the projection and pop it, then back to the model view.
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.MatrixMode(MatrixMode.Modelview);

                //reset point size
                GL.PointSize(1.0f);

                GL.Flush();
                oglMain.SwapBuffers();

                lblSpeed.Text = "???";
                lblHz.Text = " ???? \r\n Not Connected";

            }
            else if (isGPSPositionInitialized)
            {
                oglMain.MakeCurrent();

                if (isDay) GL.ClearColor(0.27f, 0.4f, 0.7f, 1.0f);
                else GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

                //  Clear the color and depth buffer.
                GL.Clear(ClearBufferMask.StencilBufferBit | ClearBufferMask.ColorBufferBit);


                GL.LoadIdentity();

                //the bounding box of the camera for cullling.
                CalcFrustum();

                //position the camera
                worldManager.SetWorldPerspective(pivotAxlePos.easting, pivotAxlePos.northing);

                int test = bnd.IsPointInsideRateArea(pivotAxlePos);
                for (int i = 0; i < bnd.Rate.Count; i++)
                {
                    if (test == i)
                        GL.Color3(0.0f, 1.0f, 0.0f);
                    else
                        GL.Color3(1.0 / bnd.Rate.Count * i, 0.0f, 1.0 / bnd.Rate.Count * (bnd.Rate.Count - i));
                    bnd.Rate[i].DrawPolyLine(DrawType.Triangles);
                }

                if (isDrawPolygons) GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);

                GL.Enable(EnableCap.Blend);

                //draw patches of sections
                foreach (Polyline2 triList in patchList)
                {
                    if (double.IsInfinity(triList.northMax))
                    {
                        triList.CalculateBoundingBox(2);
                    }

                    if (eastMin > triList.eastMax)
                        continue;
                    else if (eastMax < triList.eastMin)
                        continue;
                    else if (northMin > triList.northMax)
                        continue;
                    else if (northMax < triList.northMin)
                        continue;

                    bool isDraw = false;
                    int count2 = triList.points.Count;
                    for (int i = 2; i < count2; i += 3)
                    {
                        if ((screenRight.northing * (triList.points[i].easting - ptr.easting) - screenRight.easting * (triList.points[i].northing - ptr.northing)) <= 0)
                            continue;//right
                        if ((screenLeft.northing * (triList.points[i].easting - pbl.easting) - screenLeft.easting * (triList.points[i].northing - pbl.northing)) <= 0)
                            continue;//left
                        if ((screenBottom.northing * (triList.points[i].easting - pbl.easting) - screenBottom.easting * (triList.points[i].northing - pbl.northing)) >= 0)
                            continue;//bottom
                        if ((screenTop.northing * (triList.points[i].easting - ptr.easting) - screenTop.easting * (triList.points[i].northing - ptr.northing)) >= 0)
                            continue;//Top

                        //point is in frustum so draw the entire patch. The downside of triangle strips.
                        isDraw = true;
                        break;
                    }

                    if (isDraw && triList.points.Count > 4)
                    {
                        GL.Color4((byte)triList.points[0].easting, (byte)triList.points[0].northing, (byte)triList.points[1].easting, (byte)(isDay ? 152 : 76));

                        triList.DrawPolyLine(DrawType.TriangleStrip);
                    }
                }

                for (int j = 0; j < tool.sections.Count; j++)
                {
                    if (tool.sections[j].isMappingOn && tool.sections[j].triangleList.Count > 3)
                    {
                        GL.Color4((byte)tool.sections[j].triangleList[0].easting, (byte)tool.sections[j].triangleList[0].northing, (byte)tool.sections[j].triangleList[1].easting, (byte)(isDay ? 152 : 76));

                        //draw the triangle in each triangle strip
                        GL.Begin(PrimitiveType.TriangleStrip);

                        for (int i = 2; i < tool.sections[j].triangleList.Count; i++) GL.Vertex3(tool.sections[j].triangleList[i].easting, tool.sections[j].triangleList[i].northing, 0);

                        //left side of triangle
                        vec2 pt = new vec2((tool.cosSectionHeading * (tool.sections[j].positionLeft + tool.toolOffset)) + tool.WorkPos.easting,
                                (tool.sinSectionHeading * (tool.sections[j].positionLeft + tool.toolOffset)) + tool.WorkPos.northing);

                        GL.Vertex3(pt.easting, pt.northing, 0);

                        //Right side of triangle
                        pt = new vec2((tool.cosSectionHeading * (tool.sections[j].positionRight + tool.toolOffset)) + tool.WorkPos.easting,
                           (tool.sinSectionHeading * (tool.sections[j].positionRight + tool.toolOffset)) + tool.WorkPos.northing);

                        GL.Vertex3(pt.easting, pt.northing, 0);
                        GL.End();
                    }
                }

                if (tram.displayMode != 0)
                {
                    if (worldManager.camSetDistance < 250) GL.LineWidth(4);
                    else GL.LineWidth(2);
                    GL.Color4(0.30f, 0.93692f, 0.7520f, 0.3);
                    tram.DrawTram();
                }

                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                GL.Color3(1, 1, 1);

                //shape.Draw();

                if (bnd.bndList.Count > 0)
                {
                    //draw Boundaries
                    bnd.DrawFenceLines();

                    GL.LineWidth(gyd.lineWidth);

                    //draw the turnLines
                    if (gyd.isYouTurnBtnOn)
                    {
                        GL.Color3(0.3555f, 0.6232f, 0.20f);
                        for (int i = 0; i < bnd.bndList.Count; i++)
                        {
                            bnd.bndList[i].turnLine.DrawPolyLine(DrawType.LineLoop);
                        }
                    }

                    //Draw headland
                    if (bnd.isHeadlandOn)
                    {
                        GL.Color3(0.960f, 0.96232f, 0.30f);
                        for (int i = 0; i < bnd.bndList.Count; i++)
                        {
                            for (int j = 0; j < bnd.bndList[i].hdLine.Count; j++)
                            {
                                bnd.bndList[i].hdLine[j].DrawPolyLine(DrawType.LineLoop);
                            }
                        }
                    }
                }

                if (isDriveIn && secondsSinceStart - lastSecondInField >= 2.5)
                {
                    GL.Color3(0.95f, 0.75f, 0.50f);
                    GL.LineWidth(gyd.lineWidth);
                    for (int i = 0; i < Fields.Count; i++)
                    {
                        if (currentFieldDirectory != Fields[i].Dir)
                            Fields[i].DrawPolyLine(DrawType.LineLoop);
                    }
                }

                //draw contour line if button on
                gyd.DrawGuidanceLines();

                if (bnd.isBndBeingMade && bnd.bndBeingMadePts.points.Count > 0)
                {
                    //the boundary so far
                    GL.Color3(0.825f, 0.22f, 0.90f);
                    //int aa = 255 << 8;
                    //GL.Color3(ref aa);


                    bnd.bndBeingMadePts.DrawPolyLine(DrawType.LineStrip);

                    //line from last point to pivot marker
                    GL.Color3(0.825f, 0.842f, 0.0f);
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(1, 0x0700);
                    GL.Begin(PrimitiveType.LineStrip);

                    GL.Vertex3(bnd.bndBeingMadePts.points[0].easting, bnd.bndBeingMadePts.points[0].northing, 0);
                    GL.Vertex3(pivotAxlePos.easting + (Math.Cos(fixHeading) * (bnd.isDrawRightSide ? bnd.createBndOffset : -bnd.createBndOffset)),
                              pivotAxlePos.northing - (Math.Sin(fixHeading) * (bnd.isDrawRightSide ? bnd.createBndOffset : -bnd.createBndOffset)), 0);
                    GL.Vertex3(bnd.bndBeingMadePts.points[bnd.bndBeingMadePts.points.Count - 1].easting, bnd.bndBeingMadePts.points[bnd.bndBeingMadePts.points.Count - 1].northing, 0);

                    GL.Color3(0.295f, 0.972f, 0.290f);
                    GL.Vertex3(bnd.bndBeingMadePts.points[bnd.bndBeingMadePts.points.Count - 1].easting, bnd.bndBeingMadePts.points[bnd.bndBeingMadePts.points.Count - 1].northing, 0);

                    GL.Vertex3(bnd.bndBeingMadePts.points[0].easting, bnd.bndBeingMadePts.points[0].northing, 0);
                    GL.End();
                    GL.Disable(EnableCap.LineStipple);

                    //boundary points
                    GL.Color3(0.0f, 0.95f, 0.95f);
                    GL.PointSize(6.0f);
                    bnd.bndBeingMadePts.DrawPolyLine(DrawType.Points);
                }

                if (flagPts.Count > 0) DrawFlags();

                //Direct line to flag if flag selected
                try
                {
                    if (flagNumberPicked > 0)
                    {
                        GL.LineWidth(gyd.lineWidth);
                        GL.Enable(EnableCap.LineStipple);
                        GL.LineStipple(1, 0x0707);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Color3(0.930f, 0.72f, 0.32f);
                        GL.Vertex3(pivotAxlePos.easting, pivotAxlePos.northing, 0);
                        GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing, 0);
                        GL.End();
                        GL.Disable(EnableCap.LineStipple);
                    }
                }
                catch { }

                GL.PushMatrix();

                //draw the vehicle/implement
                tool.DrawTool();
                if (vehicleGPSWatchdog < 11)
                    vehicle.DrawVehicle();
                GL.PopMatrix();

                if (worldManager.camSetDistance < 75 && tool.isSteering)
                {
                    GL.PointSize(8.0f);
                    GL.Color3(0.0f, 0.0f, 0.0f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex3(mc.fixTool.easting, mc.fixTool.northing, 0.2);
                    GL.End();

                    GL.PointSize(4.0f);
                    GL.Color3(0.20f, 1.0f, 1.0f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Vertex3(mc.fixTool.easting, mc.fixTool.northing, 0.2);
                    GL.End();
                }

                // 2D Ortho ---------------------------------------////////-------------------------------------------------

                GL.MatrixMode(MatrixMode.Projection);
                GL.PushMatrix();
                GL.LoadIdentity();

                //negative and positive on width, 0 at top to bottom ortho view
                GL.Ortho(-(double)oglMain.Width / 2, (double)oglMain.Width / 2, (double)oglMain.Height, 0, -1, 1);

                //  Create the appropriate modelview matrix.
                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadIdentity();

                if (isSkyOn)
                    DrawSky();

                //LightBar if AB Line is set and turned on or contour
                if (isLightbarOn)
                {
                    if (vehicleGPSWatchdog < 11)
                        DrawLightBarText();

                    if (tool.isSteering)
                        DrawLightBarTextTool();
                }

                if (mc.imuRoll != 88888)
                    DrawRollBar();

                if (bnd.bndList.Count > 0 && gyd.isYouTurnBtnOn) DrawUTurnBtn();

                if (isAutoSteerBtnOn && (gyd.CurrentGMode == Mode.AB || gyd.CurrentGMode == Mode.Curve)) DrawManUTurnBtn();

                //if (isCompassOn) DrawCompass();
                DrawCompassText();

                if (isSpeedoOn) DrawSpeedo();

                DrawSteerCircle();

                if (vehicle.isHydLiftOn) DrawLiftIndicator();

                if (isReverse) DrawReverse();

                if (isRTK)
                {
                    if (mc.fixQuality != 4)
                    {
                        DrawLostRTK();
                        if (isRTK_KillAutosteer && isAutoSteerBtnOn)
                            setBtnAutoSteer(false);
                    }
                }

                if (mc.age > mc.ageAlarm) DrawAge();

                GL.Flush();//finish openGL commands
                GL.PopMatrix();//  Pop the modelview.

                ////-------------------------------------------------ORTHO END---------------------------------------

                //  back to the projection and pop it, then back to the model view.
                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix();
                GL.MatrixMode(MatrixMode.Modelview);

                //reset point size
                GL.PointSize(1.0f);
                GL.Flush();
                oglMain.SwapBuffers();

                if (leftMouseDownOnOpenGL) MakeFlagMark();

                //draw the zoom window
                if (updateZoomWindow && isJobStarted && oglZoom.Width != 400)
                {
                    oglZoom.Refresh();
                    updateZoomWindow = false;
                }
            }
        }

        private void oglBack_Load(object sender, EventArgs e)
        {
            oglBack.MakeCurrent();
            GL.Disable(EnableCap.CullFace);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 1);
            GL.ClearColor((byte)0, (byte)0, (byte)0, (byte)255);

            GL.Viewport(0, 0, 500, 300);
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(-25.0f, 25.0f, -1.0f, 29.0f, -1000.0f, 1000.0f);

            GL.MatrixMode(MatrixMode.Projection);//set state to load the projection matrix
            GL.LoadMatrix(ref projection);
            GL.MatrixMode(MatrixMode.Modelview);//set state to draw global coordinates into clip space;
            GL.EnableClientState(ArrayCap.VertexArray);
        }

        bool BrianMode = false;

        private void oglBack_Paint(object sender, PaintEventArgs e)
        {
            if (BrianMode)
            {
                tool.lookAheadMin = -1;
                tool.mappingOnDelay = 0;
            }
            bool isToolInHeadland = vehicle.isHydLiftOn && bnd.isSectionControlledByHeadland;
            int taggedHead = 0;
            int totalHead = 0;
            bool isSuperSectionAllowedOn = !tool.isMultiColoredSections;
            bool Needsupdate = true;

            //determine farthest lookahead - is the height of the readpixel line
            int rpHeight = (int)Math.Max(tool.lookAheadMax + 1.5, 1.0);
            int rpHeight2 = (int)Math.Max(Math.Min(tool.lookAheadMin - 1.5, rpHeight - 1), -1.0);

            int grnPixelsLength = tool.rpWidth * (rpHeight - rpHeight2);

            for (int j = 0; j < tool.numOfSections; j++)
            {
                if (tool.sections[j].sectionState == btnStates.On)
                {
                    tool.sections[j].sectionOnRequest = 2;
                }
                else if (tool.sections[j].sectionState == btnStates.Off)
                {
                    tool.sections[j].sectionOnRequest = 0;
                    if (tool.sections[j].sectionOverlapTimer > 0) tool.sections[j].sectionOverlapTimer = 1;
                    isSuperSectionAllowedOn &= tool.sections[j].mappingOffTimer > 1;
                }
                else if (tool.sections[j].sectionState == btnStates.Auto)
                {
                    double speeddif = (tool.toolFarRightSpeed - tool.toolFarLeftSpeed) / tool.rpWidth;

                    int start = tool.sections[j].rpSectionPosition - tool.sections[0].rpSectionPosition;
                    int end = start + tool.sections[j].rpSectionWidth;

                    double Centersp = tool.toolFarLeftSpeed + speeddif * (start + tool.sections[j].rpSectionWidth / 2.0);

                    if (Centersp * 3.6 <= tool.slowSpeedCutoff)
                    {
                        tool.sections[j].sectionOnRequest = 0;
                        if (tool.sections[j].sectionOverlapTimer > 0) tool.sections[j].sectionOverlapTimer = 1;
                        isSuperSectionAllowedOn = false;
                    }
                    else
                    {
                        if (Needsupdate)
                        {
                            UpdateBackBuffer(rpHeight, rpHeight2, false, false);
                            Needsupdate = false;
                        }
                        //calculate the slope
                        double mOn = (tool.lookAheadDistanceOnPixelsRight - tool.lookAheadDistanceOnPixelsLeft) / tool.rpWidth;
                        double mOff = (tool.lookAheadDistanceOffPixelsRight - tool.lookAheadDistanceOffPixelsLeft) / tool.rpWidth;
                        double mBoundOff = (tool.lookAheadBoundaryOffPixelsRight - tool.lookAheadBoundaryOffPixelsLeft) / tool.rpWidth;
                        double mBoundOn = (tool.lookAheadBoundaryOnPixelsRight - tool.lookAheadBoundaryOnPixelsLeft) / tool.rpWidth;
                        double mHyd = (vehicle.hydLiftLookAheadDistanceRight - vehicle.hydLiftLookAheadDistanceLeft) / tool.rpWidth;

                        if (end > tool.rpWidth) end = tool.rpWidth;

                        int totalOn = 0;
                        int totalInBound = 0;
                        for (int pos = start; pos < end; pos++)
                        {
                            int OffHeight = (int)Math.Round((tool.lookAheadDistanceOffPixelsLeft - (rpHeight2 + 1)) + (mOff * pos), MidpointRounding.AwayFromZero) * tool.rpWidth + pos;
                            int OnHeight = (int)Math.Round((tool.lookAheadDistanceOnPixelsLeft - rpHeight2) + (mOn * pos), MidpointRounding.AwayFromZero) * tool.rpWidth + pos;
                            int OffBound = (int)Math.Round((tool.lookAheadBoundaryOffPixelsLeft - (rpHeight2 + 1)) + (mBoundOff * pos), MidpointRounding.AwayFromZero) * tool.rpWidth + pos;
                            int OnBound = (int)Math.Round((tool.lookAheadBoundaryOnPixelsLeft - rpHeight2) + (mBoundOn * pos), MidpointRounding.AwayFromZero) * tool.rpWidth + pos;
                            int StopHydHeight = (int)Math.Round((vehicle.hydLiftLookAheadDistanceLeft - rpHeight2) + (mHyd * pos), MidpointRounding.AwayFromZero) * tool.rpWidth + pos;

                            int startAddress = Math.Max(0, Math.Min(Math.Min(OnHeight, OffHeight), Math.Min(OffBound, OnBound)));
                            int stopAddress = Math.Max(Math.Max(OnHeight, OffHeight), Math.Max(OffBound, OnBound));
                            if (isToolInHeadland)
                            {
                                startAddress = pos;
                                stopAddress = Math.Max(stopAddress, StopHydHeight);
                            }

                            int taggedOn = 0;
                            int taggedOff = 0;
                            int taggedOutBound = 0;
                            int taggedInBound = 0;

                            for (int a = startAddress; a <= stopAddress; a += tool.rpWidth)
                            {
                                if (a >= 0 && a <= grnPixelsLength)
                                {
                                    int Procent5 = grnPixels[a] % 5;
                                    if (a <= OffHeight + tool.rpWidth)
                                    {
                                        if (!(grnPixels[a] == 250 || grnPixels[a] == 245 || grnPixels[a] == 240 || grnPixels[a] == 235))
                                        {
                                            ++taggedOff;
                                        }
                                    }
                                    if (a <= OnHeight)
                                    {
                                        if (grnPixels[a] == 250 || grnPixels[a] == 245 || grnPixels[a] == 240 || grnPixels[a] == 235)
                                        {
                                            ++taggedOn;
                                        }
                                    }

                                    if (a <= OffBound + tool.rpWidth)
                                    {
                                        if (Procent5 == 2 || Procent5 == 1)//when outside fence dont wait for sectionOverlapTimer
                                        {
                                            ++taggedOutBound;
                                        }
                                    }
                                    if (a <= OnBound)
                                    {
                                        if (Procent5 < 1 || Procent5 > 2)
                                        {
                                            ++taggedInBound;
                                        }
                                    }

                                    if (isToolInHeadland && a < StopHydHeight)
                                    {
                                        totalHead++;
                                        if (Procent5 != 0)//when inner should also lift if (grnPixels[a] % 5 == 2 || grnPixels[a] % 5 == 4) outside only
                                            ++taggedHead;
                                    }
                                }
                            }
                            if ((taggedOn > 0 && (OnHeight < OffHeight ? (taggedOn > taggedOff) : (taggedOn >= taggedOff))) || (taggedOff == 0 && tool.sections[j].sectionOnRequest > 0))
                                totalOn++;

                            if (OnBound < OffBound ? (taggedInBound > taggedOutBound) : (taggedInBound >= taggedOutBound))
                                totalInBound++;
                        }

                        if (totalInBound == 0 || (double)totalInBound / tool.sections[j].rpSectionWidth < 1 - tool.boundOverlap)
                        {
                            tool.sections[j].sectionOnRequest = 0;
                            if (tool.sections[j].sectionOverlapTimer > 0) tool.sections[j].sectionOverlapTimer = 1;
                        }
                        else if ((totalOn > 0 && ((double)totalOn / tool.sections[j].rpSectionWidth >= 1 - tool.maxOverlap * 0.01)))
                        {
                            tool.sections[j].sectionOnRequest = 2;
                        }

                        isSuperSectionAllowedOn &= (tool.sections[j].mappingOnTimer < 2 && (tool.sections[j].mappingOffTimer > 1 || tool.sections[j].sectionOverlapTimer + tool.sections[j].mappingOffTimer > (tool.sections[j].sectionOnRequest > 0 ? 1 : 2))) || (tool.sections[j].sectionOnRequest > 0 && (tool.sections[j].isMappingOn || tool.sections[j].mappingOnTimer == 1 || HzTime * tool.lookAheadOnSetting < 1));
                    }
                }
            }
            tool.sections[tool.numOfSections].sectionOnRequest = isSuperSectionAllowedOn ? 2 : 0;

            tram.controlByte = 0;
            if (tram.displayMode != 0 && tram.isTramOnBackBuffer)
            {
                int offset = 0;
                if (Needsupdate)
                    UpdateBackBuffer(1, 0, true, false);

                else if (rpHeight2 > 0 || rpHeight + rpHeight2 < 1)
                    GL.ReadPixels(tool.rpXPosition, 10, tool.rpWidth, 1, OpenTK.Graphics.OpenGL.PixelFormat.Green, PixelType.UnsignedByte, grnPixels);
                else if (rpHeight2 < 0)
                    offset = -rpHeight2 * tool.rpWidth;

                if (tool.toolWidth > vehicle.trackWidth)
                {
                    int tramRight = grnPixels[offset + (int)(tram.isOuter ? (tool.rpWidth - tram.halfWheelTrack * 10) : (tool.rpWidth / 2 + tram.halfWheelTrack * 10))];
                    int tramLeft = grnPixels[offset + (int)(tram.isOuter ? (tram.halfWheelTrack * 10) : (tool.rpWidth / 2 - tram.halfWheelTrack * 10))];

                    int pos2 = tramLeft % 20;
                    int pos1 = tramRight % 20;

                    if (tramRight > 0 && (((pos1 > 12) && (pos1 < 20)) || (pos1 < 8))) tram.controlByte += 1;
                    if (tramLeft > 0 && (((pos2 > 12) && (pos2 < 20)) || (pos2 < 8))) tram.controlByte += 2;
                }
            }

            if (!Needsupdate)//set hydraulics based on tool in headland or not
            {
                if (vehicle.isHydLiftOn && mc.avgSpeed > 0.2 && autoBtnState == btnStates.Auto)
                {
                    if (totalHead > 0 && taggedHead >= totalHead)
                    {
                        p_239.pgn[p_239.hydLift] = 2;
                        if (!sounds.isHydLiftChange)
                        {
                            if (sounds.isHydLiftSoundOn) sounds.sndHydLiftUp.Play();
                            sounds.isHydLiftChange = true;
                        }
                    }
                    else
                    {
                        p_239.pgn[p_239.hydLift] = 1;
                        if (sounds.isHydLiftChange)
                        {
                            if (sounds.isHydLiftSoundOn) sounds.sndHydLiftDn.Play();
                            sounds.isHydLiftChange = false;
                        }
                    }
                }
            }
        }

        private void UpdateBackBuffer(int rpHeight, int rpHeight2, bool tramOnly, bool Test)
        {
            oglBack.MakeCurrent();

            GL.Disable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();// Reset The View

            double sinH = Math.Sin(tool.toolHeading);
            double cosH = Math.Cos(tool.toolHeading);

            if (!Test)
            {
                //rotate camera so heading matched fix heading in the world
                GL.Rotate(glm.toDegrees(tool.toolHeading), 0, 0, 1);

                //translate to that spot in the world
                GL.Translate(-tool.WorkPos.easting - cosH * tool.toolOffset, -tool.WorkPos.northing + sinH * tool.toolOffset, 0);

                GL.Color3((byte)0, (byte)(bnd.bndList.Count == 0 ? 250 : 252), (byte)0);
                GL.Begin(PrimitiveType.TriangleStrip);

                GL.Vertex3(tool.WorkPos.easting - 200, tool.WorkPos.northing - 200, 0);
                GL.Vertex3(tool.WorkPos.easting + 200, tool.WorkPos.northing - 200, 0);
                GL.Vertex3(tool.WorkPos.easting - 200, tool.WorkPos.northing + 200, 0);
                GL.Vertex3(tool.WorkPos.easting + 200, tool.WorkPos.northing + 200, 0);

                GL.End();
            }
            else
            {
                //translate to that spot in the world
                GL.Translate(-rpHeight, -rpHeight2, 0);

                GL.Color3((byte)0, (byte)(bnd.bndList.Count == 0 ? 250 : 252), (byte)0);
                GL.Begin(PrimitiveType.TriangleStrip);
                GL.Vertex3(rpHeight - 100, rpHeight2 - 100, 0);
                GL.Vertex3(rpHeight - 100, rpHeight2 + 100, 0);
                GL.Vertex3(rpHeight + 100, rpHeight2 - 100, 0);
                GL.Vertex3(rpHeight + 100, rpHeight2 + 100, 0);

                GL.End();
            }

            if (!tramOnly)
            {
                if (bnd.isHeadlandOn && bnd.isSectionControlledByHeadland && bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
                {
                    GL.Color3((byte)0, (byte)249, (byte)0);
                    bnd.bndList[0].fenceLine.DrawPolyLine(DrawType.Triangles);

                    GL.Color3((byte)0, (byte)250, (byte)0);
                    for (int k = 0; k < bnd.bndList[0].hdLine.Count; k++)
                    {
                        bnd.bndList[0].hdLine[k].DrawPolyLine(DrawType.Triangles);
                    }
                }
                else if (bnd.bndList.Count > 0)
                {
                    GL.Color3((byte)0, (byte)250, (byte)0);
                    bnd.bndList[0].fenceLine.DrawPolyLine(DrawType.Triangles);
                }

                for (int k = 1; k < bnd.bndList.Count; k++)
                {
                    if (bnd.isHeadlandOn && bnd.isSectionControlledByHeadland && bnd.bndList[k].hdLine.Count > 0)
                    {
                        GL.Color3((byte)0, (byte)248, (byte)0);
                        for (int l = 0; l < bnd.bndList[k].hdLine.Count; l++)
                        {
                            bnd.bndList[k].hdLine[l].DrawPolyLine(DrawType.Triangles);
                        }
                    }

                    GL.Color3((byte)0, (byte)251, (byte)0);
                    bnd.bndList[k].fenceLine.DrawPolyLine(DrawType.Triangles);
                }
            }

            GL.Enable(EnableCap.Blend);
            GL.BlendEquation(BlendEquationMode.FuncReverseSubtract);

            //draw 245 green for the tram tracks
            if (tram.isTramOnBackBuffer && tram.displayMode != 0)
            {
                GL.Color4((byte)0, (byte)5, (byte)0, (byte)0);
                GL.LineWidth(8);

                tram.DrawTram();
            }

            if (!tramOnly)
            {
                GL.Color4((byte)0, (byte)20, (byte)0, (byte)0);

                double northMax, northMin, eastMax, eastMin;

                if (!Test)
                {
                    double offsetEast = tool.WorkPos.easting - cosH * tool.toolOffset;
                    double offsetNorth = tool.WorkPos.northing + sinH * tool.toolOffset;
                    double halfToolWidthCosH = cosH * tool.toolWidth * 0.5;
                    double halfToolWidthSinH = sinH * tool.toolWidth * 0.5;
                    double ToollookAheadMaxCosH = cosH * tool.lookAheadMax * 0.1;
                    double ToollookAheadMaxSinH = sinH * tool.lookAheadMax * 0.1;
                    double ToollookAheadMinCosH = cosH * Math.Max(tool.lookAheadMin, -1.0) * 0.1;
                    double ToollookAheadMinSinH = sinH * Math.Max(tool.lookAheadMin, -1.0) * 0.1;

                    northMax = offsetNorth + (cosH >= 0 ? ToollookAheadMaxCosH : ToollookAheadMinCosH) + (sinH >= 0 ? halfToolWidthSinH : -halfToolWidthSinH);
                    northMin = offsetNorth + (cosH >= 0 ? ToollookAheadMinCosH : ToollookAheadMaxCosH) + (sinH >= 0 ? -halfToolWidthSinH : halfToolWidthSinH);

                    eastMax = offsetEast + (sinH >= 0 ? ToollookAheadMaxSinH : ToollookAheadMinSinH) + (cosH >= 0 ? halfToolWidthCosH : -halfToolWidthCosH);
                    eastMin = offsetEast + (sinH >= 0 ? ToollookAheadMinSinH : ToollookAheadMaxSinH) + (cosH >= 0 ? -halfToolWidthCosH : halfToolWidthCosH);
                }
                else
                {
                    northMax = rpHeight2 + 200;
                    northMin = rpHeight2 - 200;
                    eastMax = rpHeight + 200;
                    eastMin = rpHeight - 200;
                }

                //for every new chunk of patch
                foreach (Polyline2 triList in patchList)
                {
                    if (double.IsInfinity(triList.northMax))
                    {
                        triList.CalculateBoundingBox(2);
                    }

                    if (eastMin > triList.eastMax)
                        continue;
                    if (eastMax < triList.eastMin)
                        continue;
                    if (northMin > triList.northMax)
                        continue;
                    if (northMax < triList.northMin)
                        continue;

                    bool isDraw = false;
                    int count2 = triList.points.Count;
                    for (int i = 2; i < count2; i += 3)
                    {
                        //determine if point is in frustum or not
                        if (triList.points[i].easting > pivotAxlePos.easting + 50)
                            continue;
                        if (triList.points[i].easting < pivotAxlePos.easting - 50)
                            continue;
                        if (triList.points[i].northing > pivotAxlePos.northing + 50)
                            continue;
                        if (triList.points[i].northing < pivotAxlePos.northing - 50)
                            continue;

                        //point is in frustum so draw the entire patch
                        isDraw = true;
                        break;
                    }
                    if (isDraw && triList.points.Count > 4)
                    {
                        triList.DrawPolyLine(DrawType.TriangleStrip);
                    }
                }

                for (int j = 0; j < tool.sections.Count; j++)
                {
                    int patchCount = tool.sections[j].triangleList.Count;
                    // the follow up to sections patches
                    if (tool.sections[j].isMappingOn && patchCount > 3)
                    {
                        //draw the triangle in each triangle strip
                        GL.Begin(PrimitiveType.TriangleStrip);

                        for (int k = 2; k < patchCount; k++)
                        {
                            GL.Vertex3(tool.sections[j].triangleList[k].easting, tool.sections[j].triangleList[k].northing, 0);
                        }
                        GL.Vertex3(tool.sections[j].leftPoint.easting - Math.Sin(tool.toolHeading) * 0.1, tool.sections[j].leftPoint.northing - Math.Cos(tool.toolHeading) * 0.1, 0);
                        GL.Vertex3(tool.sections[j].rightPoint.easting - Math.Sin(tool.toolHeading) * 0.1, tool.sections[j].rightPoint.northing - Math.Cos(tool.toolHeading) * 0.1, 0);

                        GL.End();
                    }
                }
            }

            //finish it up - we need to read the ram of video card
            GL.Flush();
            if (!Test)
                //read the whole block of pixels up to max lookahead, one read only
                GL.ReadPixels(tool.rpXPosition, 10 + rpHeight2, tool.rpWidth, rpHeight - rpHeight2, OpenTK.Graphics.OpenGL.PixelFormat.Green, PixelType.UnsignedByte, grnPixels);
            else
                GL.ReadPixels(0, 0, 500, 300, OpenTK.Graphics.OpenGL.PixelFormat.Green, PixelType.UnsignedByte, grnPixels);

            //Outside        = 252
            //inside inner   = 251
            //inside field   = 250
            //headland       = 249
            //headland inner = 248

            //tram           = -5 max 3x overlap
            //section        = -20 max 11x overlap

            if (Debugger.IsAttached)
            {
                if (!Debugger.IsAttached || false)
                {
                    //Paint to context for troubleshooting
                    oglBack.BringToFront();
                    oglBack.SwapBuffers();
                }
                else oglBack.SendToBack();
            }
        }

        private void oglZoom_Load(object sender, EventArgs e)
        {
            oglZoom.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 1);

            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ClearColor(0, 0, 0, 1.0f);
            GL.EnableClientState(ArrayCap.VertexArray);
        }

        private void oglZoom_Resize(object sender, EventArgs e)
        {
            oglZoom.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Viewport(0, 0, oglZoom.Width, oglZoom.Height);
            Matrix4 mat = Matrix4.CreateOrthographic((float)maxFieldDistance, (float)maxFieldDistance, -1.0f, 1.0f);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void oglZoom_Paint(object sender, PaintEventArgs e)
        {
            if (isJobStarted)
            {
                if (Debugger.IsAttached && false)
                {
                    double total = 0;
                    double outside = 0;
                    double ininner = 0;
                    double infield = 0;
                    double inhead0 = 0;
                    double inhead1 = 0;
                    double tramcnt = 0;
                    double section0x = 0;
                    double section1x = 0;
                    double section2x = 0;
                    double section3x = 0;
                    double section4x = 0;
                    double section5x = 0;
                    double section6x = 0;
                    double section7x = 0;
                    double section8x = 0;
                    double section9x = 0;
                    double section10x = 0;
                    double section11x = 0;
                    double unknown = 0;

                    for (int X = (int)(minFieldX - 1); X <= maxFieldX + 25; X += 50)
                    {
                        for (int Y = (int)(minFieldY - 1); Y <= maxFieldY + 15; Y += 30)
                        {
                            UpdateBackBuffer(X, Y, false, true);

                            total += grnPixels.Length - 1;
                            for (int a = 0; a < grnPixels.Length; a++)
                            {
                                int dfdf = grnPixels[a];
                                int tt4 = grnPixels[a] % 5;
                                if (grnPixels[a] < 13 || grnPixels[a] > 252)
                                {
                                    unknown++;
                                }
                                else
                                {
                                    if ((grnPixels[a] % 20 >= 0 && grnPixels[a] % 20 < 8) || (grnPixels[a] % 20 > 12 && grnPixels[a] % 20 < 20))
                                        tramcnt++;

                                    if (tt4 == 0)
                                        infield++;
                                    else if (tt4 == 1)
                                        ininner++;
                                    else if (tt4 == 2)
                                        outside++;
                                    else if (tt4 == 3)
                                        inhead1++;
                                    else if (tt4 == 4)
                                        inhead0++;

                                    if (grnPixels[a] > 232)
                                        section0x++;
                                    else if (grnPixels[a] > 212)
                                        section1x++;
                                    else if (grnPixels[a] > 192)
                                        section2x++;
                                    else if (grnPixels[a] > 172)
                                        section3x++;
                                    else if (grnPixels[a] > 152)
                                        section4x++;
                                    else if (grnPixels[a] > 132)
                                        section5x++;
                                    else if (grnPixels[a] > 112)
                                        section6x++;
                                    else if (grnPixels[a] > 92)
                                        section7x++;
                                    else if (grnPixels[a] > 72)
                                        section8x++;
                                    else if (grnPixels[a] > 52)
                                        section9x++;
                                    else if (grnPixels[a] > 32)
                                        section10x++;
                                    else if (grnPixels[a] > 12)
                                        section11x++;
                                }
                            }
                        }
                    }

                    double total2 = section1x + section2x + section3x + section4x + section5x + section6x + section7x + section8x + section9x + section10x + section11x;
                    double total3 = section2x + section3x * 2 + section4x * 3 + section5x * 4 + section6x * 5 + section7x * 6 + section8x * 7 + section9x * 8 + section10x * 9 + section11x * 10;
                    double totalinfield = infield + inhead1 + inhead0;

                    bnd.actualAreaCovered = (total2 / totalinfield);
                    bnd.overlapPercent = Math.Round(((total3 / total2) * 100), 2);
                }

                if (oglZoom.Width != 400)
                {
                    oglZoom.MakeCurrent();

                    GL.Disable(EnableCap.Blend);

                    GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                    GL.LoadIdentity();                  // Reset The View

                    //translate to that spot in the world 
                    GL.Translate(-fieldCenterX, -fieldCenterY, 0);

                    GL.Color3(sectionColor.R, sectionColor.G, sectionColor.B);

                    //for every new chunk of patch
                    foreach (Polyline2 triList in patchList)
                    {
                        triList.DrawPolyLine(DrawType.TriangleStrip);
                    }

                    //draw curve if there is one
                    if (gyd.currentGuidanceLine != null && gyd.curList.points.Count > 1)
                    {
                        GL.LineWidth(2);
                        GL.Color3(0.925f, 0.2f, 0.90f);
                        GL.Begin(gyd.curList.loop ? PrimitiveType.LineLoop : PrimitiveType.LineStrip);
                        for (int h = 0; h < gyd.curList.points.Count; h++) GL.Vertex3(gyd.curList.points[h].easting, gyd.curList.points[h].northing, 0);
                        GL.End();
                    }

                    //draw all the fences
                    bnd.DrawFenceLines();

                    GL.PointSize(8.0f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Color3(0.95f, 0.90f, 0.0f);
                    GL.Vertex3(pivotAxlePos.easting, pivotAxlePos.northing, 0.0);
                    GL.End();

                    GL.PointSize(1.0f);

                    GL.Flush();
                    oglZoom.MakeCurrent();
                    oglZoom.SwapBuffers();
                }
            }
        }

        private void DrawManUTurnBtn()
        {
            GL.Enable(EnableCap.Texture2D);

            if (isUTurnOn)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[5]);        // Select Our Texture
                GL.Color3(0.90f, 0.90f, 0.293f);

                int two3 = oglMain.Width / 4;
                GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
                {
                    GL.TexCoord2(0, 0); GL.Vertex2(-82 - two3, 30); // 
                    GL.TexCoord2(1, 0); GL.Vertex2(82 - two3, 30); // 
                    GL.TexCoord2(1, 1); GL.Vertex2(82 - two3, 90); // 
                    GL.TexCoord2(0, 1); GL.Vertex2(-82 - two3, 90); //
                }
                GL.End();
            }

            //lateral line move

            if (isLateralOn)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[19]);        // Select Our Texture
                GL.Color3(0.190f, 0.90f, 0.93f);
                int two3 = oglMain.Width / 4;
                GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
                {
                    GL.TexCoord2(0, 0); GL.Vertex2(-82 - two3, 90); // 
                    GL.TexCoord2(1, 0); GL.Vertex2(82 - two3, 90); // 
                    GL.TexCoord2(1, 1); GL.Vertex2(82 - two3, 150); // 
                    GL.TexCoord2(0, 1); GL.Vertex2(-82 - two3, 150); //
                }
                GL.End();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        private void DrawUTurnBtn()
        {
            GL.Enable(EnableCap.Texture2D);

            if (!gyd.isYouTurnTriggered)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[3]);        // Select Our Texture
                if (distancePivotToTurnLine > 0 && !gyd.isOutOfBounds) GL.Color3(0.3f, 0.95f, 0.3f);
                else GL.Color3(0.97f, 0.635f, 0.4f);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, texture[4]);        // Select Our Texture
                GL.Color3(0.90f, 0.90f, 0.293f);
            }

            int two3 = oglMain.Width / 5;
            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            if (gyd.isYouTurnRight)
            {
                GL.TexCoord2(0, 0); GL.Vertex2(-62 + two3, 40); // 
                GL.TexCoord2(1, 0); GL.Vertex2(62 + two3, 40); // 
                GL.TexCoord2(1, 1); GL.Vertex2(62 + two3, 110); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-62 + two3, 110); //
            }
            else
            {
                GL.TexCoord2(1, 0); GL.Vertex2(-62 + two3, 40); // 
                GL.TexCoord2(0, 0); GL.Vertex2(62 + two3, 40); // 
                GL.TexCoord2(0, 1); GL.Vertex2(62 + two3, 110); // 
                GL.TexCoord2(1, 1); GL.Vertex2(-62 + two3, 110); //
            }
            //
            GL.End();
            GL.Disable(EnableCap.Texture2D);
            // Done Building Triangle Strip
            if (!gyd.isYouTurnTriggered)
            {
                if (distancePivotToTurnLine > 0)
                    font.DrawText(-30 + two3, 80, (distancePivotToTurnLine * glm.mToUserBig).ToString("0") + glm.unitsFtM);
                else
                    font.DrawText(-30 + two3, 80, "--");
            }
            else
            {
                font.DrawText(-30 + two3, 80, ((gyd.totalUTurnLength - gyd.onA) * glm.mToUserBig).ToString("0") + glm.unitsFtM);
            }
        }

        private void DrawSteerCircle()
        {
            int sizer = oglMain.Height / 15;
            int center = oglMain.Width / 2 - sizer;
            int bottomSide = oglMain.Height - sizer;

            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[11]);        // Select Our Texture

            if (mc.steerSwitchHigh)
                GL.Color4(0.9752f, 0.0f, 0.03f, 0.98);
            else if (isAutoSteerBtnOn)
                GL.Color4(0.052f, 0.970f, 0.03f, 0.97);
            else
                GL.Color4(0.952f, 0.750f, 0.03f, 0.97);

            //we have lost connection to steer module
            if (steerModuleConnectedCounter++ > 30)
            {
                GL.Color4(0.952f, 0.093570f, 0.93f, 0.97);
            }

            GL.Translate(center, bottomSide, 0);
            GL.Rotate(mc.actualSteerAngleDegrees * 2, 0, 0, 1);

            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(0, 0); GL.Vertex2(-sizer, -sizer); // 
                GL.TexCoord2(1, 0); GL.Vertex2(sizer, -sizer); // 
                GL.TexCoord2(1, 1); GL.Vertex2(sizer, sizer); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-sizer, sizer); //
            }
            GL.End();
            GL.PopMatrix();

            //Pinion
            GL.BindTexture(TextureTarget.Texture2D, texture[12]);        // Select Our Pinion
            GL.PushMatrix();

            GL.Translate(center, bottomSide, 0);
            //GL.Rotate(mc.actualSteerAngleDegrees * 2, 0, 0, 1);

            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(0, 0); GL.Vertex2(-sizer, -sizer); // 
                GL.TexCoord2(1, 0); GL.Vertex2(sizer, -sizer); // 
                GL.TexCoord2(1, 1); GL.Vertex2(sizer, sizer); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-sizer, sizer); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }

        private void MakeFlagMark()
        {
            leftMouseDownOnOpenGL = false;
            byte[] data1 = new byte[256];

            //scan the center of click and a set of square points around
            GL.ReadPixels(mouseX - 8, mouseY - 8, 16, 16, PixelFormat.StencilIndex, PixelType.UnsignedByte, data1);

            //made it here so no flag found
            flagNumberPicked = 0;

            for (int ctr = 0; ctr < 256; ctr++)
            {
                if (data1[ctr] != 0)
                {
                    flagNumberPicked = data1[ctr];
                    break;
                }
            }

            if (flagNumberPicked > 0)
            {
                Form fc = Application.OpenForms["FormFlags"];

                if (fc != null)
                {
                    fc.Focus();
                    return;
                }

                if (flagPts.Count > 0)
                {
                    Form form = new FormFlags(this);
                    form.Show(this);
                }
            }
        }

        private void DrawFlags()
        {
            GL.Enable(EnableCap.StencilTest);
            GL.PointSize(8.0f);

            int flagCnt = flagPts.Count;
            for (int f = 0; f < flagCnt; f++)
            {
                GL.StencilFunc(StencilFunction.Always, f + 1, 0xFF);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

                GL.Begin(PrimitiveType.Points);
                if (flagPts[f].color == 0) GL.Color3((byte)255, (byte)0, (byte)0);
                if (flagPts[f].color == 1) GL.Color3((byte)0, (byte)255, (byte)0);
                if (flagPts[f].color == 2) GL.Color3((byte)255, (byte)255, (byte)0);

                GL.Vertex3(flagPts[f].easting, flagPts[f].northing, 0);
                GL.End();

                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
                font.DrawText3D(flagPts[f].easting, flagPts[f].northing, "&" + flagPts[f].notes);
                //else
                //    font.DrawText3D(flagPts[f].easting, flagPts[f].northing, "&");
            }

            GL.Disable(EnableCap.StencilTest);

            if (flagNumberPicked != 0)
            {
                ////draw the box around flag
                double offSet = (worldManager.camSetDistance * 0.01);
                GL.LineWidth(4);
                GL.Color3(0.980f, 0.0f, 0.980f);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing + offSet, 0);
                GL.Vertex3(flagPts[flagNumberPicked - 1].easting - offSet, flagPts[flagNumberPicked - 1].northing, 0);
                GL.Vertex3(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing - offSet, 0);
                GL.Vertex3(flagPts[flagNumberPicked - 1].easting + offSet, flagPts[flagNumberPicked - 1].northing, 0);
                GL.End();
            }
        }

        int LightbarVBO = 0;


        private void UpdateOpenGLVBO()
        {

            double down = 13;
            LightbarVBO = GL.GenBuffer();
            short[] TT = new short[34];
            int offset = 32;

            TT[0] = (short)(-8 * offset);
            TT[1] = (short)(down);
            TT[2] = (short)(-7 * offset);
            TT[3] = (short)(down);
            TT[4] = (short)(-6 * offset);
            TT[5] = (short)(down);
            TT[6] = (short)(-5 * offset);
            TT[7] = (short)(down);
            TT[8] = (short)(-4 * offset);
            TT[9] = (short)(down);
            TT[10] = (short)(-3 * offset);
            TT[11] = (short)(down);
            TT[12] = (short)(-2 * offset);
            TT[13] = (short)(down);
            TT[14] = (short)(-1 * offset);
            TT[15] = (short)(down);
            TT[16] = (short)(0);
            TT[17] = (short)(down);
            TT[18] = (short)(1 * offset);
            TT[19] = (short)(down);
            TT[20] = (short)(2 * offset);
            TT[21] = (short)(down);
            TT[22] = (short)(3 * offset);
            TT[23] = (short)(down);
            TT[24] = (short)(4 * offset);
            TT[25] = (short)(down);
            TT[26] = (short)(5 * offset);
            TT[27] = (short)(down);
            TT[28] = (short)(6 * offset);
            TT[29] = (short)(down);
            TT[30] = (short)(7 * offset);
            TT[31] = (short)(down);
            TT[32] = (short)(8 * offset);
            TT[33] = (short)(down);


            GL.BindBuffer(BufferTarget.ArrayBuffer, LightbarVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, TT.Length * sizeof(short), TT, BufferUsageHint.StaticDraw);


        }

        private void DrawLightBar(double offlineDistance)
        {
            if (LightbarVBO == 0)
                UpdateOpenGLVBO();

            //  Dot distance is representation of how far from AB Line
            int dotDistance = (int)(offlineDistance / -lightbarCmPerPixel + (offlineDistance > 0 ? -0.5 : 0.5));

            if (dotDistance < -8) dotDistance = -8;
            if (dotDistance > 8) dotDistance = 8;


            GL.BindBuffer(BufferTarget.ArrayBuffer, LightbarVBO);
            GL.VertexPointer(2, VertexPointerType.Short, 0, 0);

            GL.PointSize(8.0f);
            GL.Color3(0.00f, 0.0f, 0.0f);
            GL.DrawArrays(PrimitiveType.Points, 0, 17);

            if (dotDistance >= 0)
            {
                GL.PointSize(4.0f);
                GL.Color3(0.980f, 0.30f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 0, 8);

                GL.Color3(0.0f, 0.9750f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 9 + dotDistance, 8 - dotDistance);

                if (dotDistance > 0)
                {
                    GL.PointSize(24.0f);
                    GL.Color3(0.0f, 0.0f, 0.0f);
                    GL.DrawArrays(PrimitiveType.Points, 9, dotDistance);

                    GL.PointSize(16.0f);
                    GL.Color3(0.0f, 0.9750f, 0.0f);
                    GL.DrawArrays(PrimitiveType.Points, 9, dotDistance);
                }
            }

            if (dotDistance <= 0)
            {
                GL.PointSize(4.0f);
                GL.Color3(0.0f, 0.9750f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 9, 8);

                GL.Color3(0.980f, 0.30f, 0.0f);
                GL.DrawArrays(PrimitiveType.Points, 0, 8 + dotDistance);

                if (dotDistance < 0)
                {
                    GL.PointSize(24.0f);
                    GL.Color3(0.0f, 0.0f, 0.0f);
                    GL.DrawArrays(PrimitiveType.Points, 8 + dotDistance, -dotDistance);

                    GL.PointSize(16.0f);
                    GL.Color3(0.980f, 0.30f, 0.0f);
                    GL.DrawArrays(PrimitiveType.Points, 8 + dotDistance, -dotDistance);
                }
            }

            GL.PointSize(dotDistance == 0 ? 24.0f : 12.0f);
            GL.Color3(0.00f, 0.0f, 0.0f);
            GL.DrawArrays(PrimitiveType.Points, 8, 1);

            GL.PointSize(dotDistance == 0 ? 16.0f : 8.0f);
            GL.Color3(0.980f, 0.98f, 0.0f);
            GL.DrawArrays(PrimitiveType.Points, 8, 1);
        }

        private void DrawLightBarText()
        {
            if (!double.IsNaN(guidanceLineDistanceOff))
            {
                //save distance for display in millimeters
                avgPivDistance = avgPivDistance * 0.5 + guidanceLineDistanceOff * 0.5;

                double avgPivotDistance = avgPivDistance * glm.mToUser;
                string hede;

                DrawLightBar(avgPivotDistance);

                if (avgPivotDistance > 0.0)
                {
                    GL.Color3(0.9752f, 0.50f, 0.3f);
                    hede = "< " + (Math.Abs(avgPivotDistance)).ToString("0");
                }
                else
                {
                    GL.Color3(0.50f, 0.952f, 0.3f);
                    hede = (Math.Abs(avgPivotDistance)).ToString("0") + " >";
                }

                int center = -(int)(((double)(hede.Length) * 0.5) * 16);
                font.DrawText(center, 30, hede, 1);
            }
        }

        private void DrawLightBarTextTool()
        {
            GL.Disable(EnableCap.DepthTest);

            if (!double.IsNaN(guidanceLineDistanceOffTool))
            {
                // in millimeters
                avgPivDistanceTool = avgPivDistanceTool * 0.5 + guidanceLineDistanceOffTool * 0.5;

                double avgPivotDistance2 = avgPivDistanceTool * glm.mToUser;
                string hede;

                //DrawLightBar(oglMain.Width, oglMain.Height, avgPivotDistance2);

                if (avgPivotDistance2 > 0.0)
                {
                    GL.Color3(0.9752f, 0.50f, 0.3f);
                    hede = "< " + (Math.Abs(avgPivotDistance2)).ToString("0");
                }
                else
                {
                    GL.Color3(0.50f, 0.952f, 0.3f);
                    hede = (Math.Abs(avgPivotDistance2)).ToString("0") + " >";
                }

                int center = -(int)(((double)(hede.Length) * 0.5) * 16);
                font.DrawText(center, 50, hede, 1);
            }
        }

        private void DrawRollBar()
        {
            if (LightbarVBO == 0)
                UpdateOpenGLVBO();

            GL.PushMatrix();
            GL.Translate(0, 100, 0);

            GL.LineWidth(1);
            GL.Color3(0.24f, 0.64f, 0.74f);
            double wiid = 60;

            //If roll is used rotate graphic based on roll angle

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-wiid - 30, 0);
            GL.Vertex2(-wiid - 2, 0);
            GL.Vertex2(wiid + 2, 0);
            GL.Vertex2(wiid + 30, 0);
            GL.End();

            mc.imuRoll = 5;

            GL.Rotate(mc.imuRoll, 0.0f, 0.0f, 1.0f);

            GL.Color3(0.74f, 0.74f, 0.14f);
            GL.LineWidth(2);

            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(-wiid + 10, 15);
            GL.Vertex2(-wiid, 0);
            GL.Vertex2(wiid, 0);
            GL.Vertex2(wiid - 10, 15);
            GL.End();

            string head = mc.imuRoll.ToString("0.0");
            int center = -(int)(((head.Length) * 6));

            font.DrawText(center, 0, head, 0.8);

            //return back
            GL.PopMatrix();
            GL.LineWidth(1);
        }

        private void DrawSky()
        {
            //GL.Translate(0, 0, 0.9);
            ////draw the background when in 3D
            if (worldManager.camPitch < -52)
            {
                //-10 to -32 (top) is camera pitch range. Set skybox to line up with horizon 
                double hite = (worldManager.camPitch + 66) * -0.025;

                //the background
                double winLeftPos = -(double)oglMain.Width / 2;
                double winRightPos = -winLeftPos;

                if (isDay)
                {
                    GL.Color3(0.75, 0.75, 0.75);
                    GL.BindTexture(TextureTarget.Texture2D, texture[0]);        // Select Our Texture
                }
                else
                {
                    GL.Color3(0.5, 0.5, 0.5);
                    GL.BindTexture(TextureTarget.Texture2D, texture[10]);        // Select Our Texture
                }

                GL.Enable(EnableCap.Texture2D);

                double u = (fixHeading) / glm.twoPI;
                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                GL.TexCoord2(u + 0.25, 0); GL.Vertex2(winRightPos, 0.0); // Top Right
                GL.TexCoord2(u, 0); GL.Vertex2(winLeftPos, 0.0); // Top Left
                GL.TexCoord2(u + 0.25, 1); GL.Vertex2(winRightPos, hite * oglMain.Height); // Bottom Right
                GL.TexCoord2(u, 1); GL.Vertex2(winLeftPos, hite * oglMain.Height); // Bottom Left
                GL.End();                       // Done Building Triangle Strip

                //GL.BindTexture(TextureTarget.Texture2D, texture[3]);		// Select Our Texture
                // GL.Translate(400, 200, 0);
                //GL.Rotate(camHeading, 0, 0, 1);
                //GL.Begin(PrimitiveType.TriangleStrip);				// Build Quad From A Triangle Strip
                //GL.TexCoord2(1, 0); GL.Vertex2(0.1 * winRightPos, -0.1 * Height); // Top Right
                //GL.TexCoord2(0, 0); GL.Vertex2(0.1 * winLeftPos, -0.1 * Height); // Top Left
                //GL.TexCoord2(1, 1); GL.Vertex2(0.1 * winRightPos, 0.1 * Height); // Bottom Right
                //GL.TexCoord2(0, 1); GL.Vertex2(0.1 * winLeftPos,  0.1 * Height); // Bottom Left
                //GL.End();						// Done Building Triangle Strip

                //disable, straight color
                GL.Disable(EnableCap.Texture2D);
            }
        }

        private void DrawCompassText()
        {
            int center = oglMain.Width / -2;

            GL.LineWidth(6);
            GL.Color3(0, 0.0, 0);
            GL.Begin(PrimitiveType.Lines);
            //-
            GL.Vertex3(-center - 17, 140, 0);
            GL.Vertex3(-center - 39, 140, 0);

            //+
            GL.Vertex3(-center - 17, 50, 0);
            GL.Vertex3(-center - 39, 50, 0);

            GL.Vertex3(-center - 27, 39, 0);
            GL.Vertex3(-center - 27, 60, 0);

            GL.End();
            GL.LineWidth(2);
            GL.Color3(0, 0.9, 0);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(-center - 17, 140, 0);
            GL.Vertex3(-center - 39, 140, 0);

            GL.Vertex3(-center - 18, 50, 0);
            GL.Vertex3(-center - 38, 50, 0);

            GL.Vertex3(-center - 27, 40, 0);
            GL.Vertex3(-center - 27, 61, 0);
            GL.End();

            GL.Color3(0.9752f, 0.952f, 0.93f);

            font.DrawText(center + 10, 20, glm.toDegrees(fixHeading).ToString("0.0"), 1);

            if (mc.imuHeading != 99999)
            {
                if (!isSuperSlow) GL.Color3(0.98f, 0.972f, 0.59903f);
                else GL.Color3(0.298f, 0.972f, 0.99903f);

                font.DrawText(center, 80, "IMU:" + mc.imuHeading.ToString("0.0"), 0.8);
                //font.DrawText(center, 110, "R:" + ahrs.imuRoll.ToString("0.0"), 0.8);
                //font.DrawText(center, 135, "Y:" + ahrs.imuYawRate.ToString("0.0"), 0.8);
            }

            //GL.Color3(0.9652f, 0.9752f, 0.1f);
            //font.DrawText(center, 150, "BETA 5.0.0.5", 1);

            GL.Color3(0.9752f, 0.62f, 0.325f);
            if (glm.isSimEnabled) font.DrawText(-110, oglMain.Height - 130, "Simulator On", 1);

            if (gyd.CurrentGMode == Mode.Contour && gyd.isLocked && isFlashOnOff)
            {
                GL.Color3(0.9652f, 0.752f, 0.75f);
                font.DrawText(-center - 100, oglMain.Height / 2.3, "Locked", 1);
            }

            //GL.Color3(0.9752f, 0.52f, 0.23f);
            //font.DrawText(center, 100, "Free Till 15 Mar", 1.0);


            //if (isFixHolding) font.DrawText(center, 110, "Holding", 0.8);

            //GL.Color3(0.9752f, 0.952f, 0.0f);
            //font.DrawText(center, 130, "Beta v4.2.02", 1.0);
        }


        private void DrawReverse()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[9]);        // Select Our Texture

            GL.Translate(-oglMain.Width / 6, oglMain.Height / 2, 0);

            GL.Rotate(180, 0, 0, 1);
            GL.Color3(0.952f, 0.0f, 0.0f);

            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(0, 0.1); GL.Vertex2(-48, -48); // 
                GL.TexCoord2(1, 0.1); GL.Vertex2(48, -48.0); // 
                GL.TexCoord2(1, 1); GL.Vertex2(48, 48); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-48, 48); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();
        }
        private void DrawLiftIndicator()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[9]);        // Select Our Texture

            GL.Translate(oglMain.Width / 2 - 35, oglMain.Height / 2, 0);

            if (p_239.pgn[p_239.hydLift] == 2)
            {
                GL.Color3(0.0f, 0.950f, 0.0f);
            }
            else
            {
                GL.Rotate(180, 0, 0, 1);
                GL.Color3(0.952f, 0.40f, 0.0f);
            }

            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(0, 0); GL.Vertex2(-48, -64); // 
                GL.TexCoord2(1, 0); GL.Vertex2(48, -64.0); // 
                GL.TexCoord2(1, 1); GL.Vertex2(48, 64); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-48, 64); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();

        }
        private void DrawSpeedo()
        {
            GL.PushMatrix();
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, texture[7]);        // Select Our Texture
            GL.Color4(0.952f, 0.980f, 0.98f, 0.99);

            GL.Translate(oglMain.Width / 2 - 65, 65, 0);

            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(0, 0); GL.Vertex2(-58, -58); // 
                GL.TexCoord2(1, 0); GL.Vertex2(58, -58.0); // 
                GL.TexCoord2(1, 1); GL.Vertex2(58, 58); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-58, 58); //
            }
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, texture[8]);        // Select Our Texture

            double angle = 0;
            double aveSpd = Math.Abs(mc.avgSpeed * glm.KMHToUser);
            if (aveSpd > 20) aveSpd = 20;
            angle = (aveSpd - 10) * 15;

            if (mc.avgSpeed > -0.1) GL.Color3(0.850f, 0.950f, 0.30f);
            else GL.Color3(0.952f, 0.0f, 0.0f);

            GL.Rotate(angle, 0, 0, 1);
            GL.Begin(PrimitiveType.Quads);              // Build Quad From A Triangle Strip
            {
                GL.TexCoord2(0, 0); GL.Vertex2(-48, -48); // 
                GL.TexCoord2(1, 0); GL.Vertex2(48, -48.0); // 
                GL.TexCoord2(1, 1); GL.Vertex2(48, 48); // 
                GL.TexCoord2(0, 1); GL.Vertex2(-48, 48); //
            }
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            GL.PopMatrix();

        }
        private void DrawLostRTK()
        {
            GL.Color3(0.9752f, 0.52f, 0.0f);
            font.DrawText(-oglMain.Width / 4, 110, "Lost RTK", 2.0);
        }

        private void DrawAge()
        {
            GL.Color3(0.9752f, 0.52f, 0.0f);
            font.DrawText(oglMain.Width / 4, 60, "Age:" + mc.age.ToString("0.0"), 1.5);
        }

        public vec2 pbl, pbr, ptl, ptr;
        private void CalcFrustum()
        {
            //the bounding box of the camera for cullling without grabbing gl matrixes
            inv_view = Matrix4.CreatePerspectiveFieldOfView(fovy, oglMain.AspectRatio, 1.0f, (float)(camDistanceFactor * worldManager.camSetDistance));
            inv_view.Invert();

            double radhead = glm.toRadians(worldManager.camHeading);
            double radpitch = glm.toRadians(worldManager.camPitch);
            float coshead = (float)Math.Cos(glm.toRadians(worldManager.camHeading));
            float sinhead = (float)Math.Sin(glm.toRadians(worldManager.camHeading));
            float cosPitch = (float)Math.Cos(glm.toRadians(worldManager.camPitch));
            float sinPitch = (float)Math.Sin(glm.toRadians(worldManager.camPitch));
            float eastsinhead = (float)pivotAxlePos.easting * sinhead;
            float northcoshead = (float)pivotAxlePos.northing * coshead;
            
            inv_proj = new Matrix4(
                new Vector4(coshead, sinhead * cosPitch, sinhead * sinPitch, 0),
                new Vector4(-sinhead, cosPitch * coshead, sinPitch * coshead, 0),
                new Vector4(0, -sinPitch, cosPitch, 0),
                new Vector4((float)(-pivotAxlePos.easting * coshead + pivotAxlePos.northing * sinhead), (float)((-eastsinhead + -northcoshead) * cosPitch), (float)((worldManager.camSetDistance * -0.5) + (eastsinhead + northcoshead) * -sinPitch), 1));

            inv_proj.Invert();

            Vector4 ftl = NDCToWorld(new Vector4(-1, 1, 1, 1), inv_view, inv_proj);
            Vector4 ftr = NDCToWorld(new Vector4(1, 1, 1, 1), inv_view, inv_proj);
            Vector4 fbl = NDCToWorld(new Vector4(-1, -1, 1, 1), inv_view, inv_proj);
            Vector4 fbr = NDCToWorld(new Vector4(1, -1, 1, 1), inv_view, inv_proj);

            Vector4 ntl = NDCToWorld(new Vector4(-1, 1, -1, 1), inv_view, inv_proj);
            Vector4 ntr = NDCToWorld(new Vector4(1, 1, -1, 1), inv_view, inv_proj);
            Vector4 nbl = NDCToWorld(new Vector4(-1, -1, -1, 1), inv_view, inv_proj);
            Vector4 nbr = NDCToWorld(new Vector4(1, -1, -1, 1), inv_view, inv_proj);

            if (nbl.Z > 0 && fbl.Z < 0)
            {
                pbl = new vec2(nbl.Z * (nbl.X - fbl.X) / (fbl.Z - nbl.Z) + nbl.X, nbl.Z * (nbl.Y - fbl.Y) / (fbl.Z - nbl.Z) + nbl.Y);
                pbr = new vec2(nbr.Z * (nbr.X - fbr.X) / (fbr.Z - nbr.Z) + nbr.X, nbr.Z * (nbr.Y - fbr.Y) / (fbr.Z - nbr.Z) + nbr.Y);
                if (ntl.Z > 0)
                {
                    if (ftl.Z < 0)
                    {
                        ptl = new vec2(ntl.Z * (ntl.X - ftl.X) / (ftl.Z - ntl.Z) + ntl.X, ntl.Z * (ntl.Y - ftl.Y) / (ftl.Z - ntl.Z) + ntl.Y);
                        ptr = new vec2(ntr.Z * (ntr.X - ftr.X) / (ftr.Z - ntr.Z) + ntr.X, ntr.Z * (ntr.Y - ftr.Y) / (ftr.Z - ntr.Z) + ntr.Y);
                    }
                    else
                    {
                        ptl = new vec2(ftl.Z * (ftl.X - fbl.X) / (fbl.Z - ftl.Z) + ftl.X, ftl.Z * (ftl.Y - fbl.Y) / (fbl.Z - ftl.Z) + ftl.Y);
                        ptr = new vec2(ftr.Z * (ftr.X - fbr.X) / (fbr.Z - ftr.Z) + ftr.X, ftr.Z * (ftr.Y - fbr.Y) / (fbr.Z - ftr.Z) + ftr.Y);
                    }
                }
            }

            screenRight = new vec2(pbr.easting - ptr.easting, pbr.northing - ptr.northing);
            screenLeft = new vec2(ptl.easting - pbl.easting, ptl.northing - pbl.northing);
            screenBottom = new vec2(pbr.easting - pbl.easting, pbr.northing - pbl.northing);
            screenTop = new vec2(ptl.easting - ptr.easting, ptl.northing - ptr.northing);

            eastMin = Math.Min(Math.Min(ptl.easting, ptr.easting), Math.Min(pbl.easting, pbr.easting));
            eastMax = Math.Max(Math.Max(ptl.easting, ptr.easting), Math.Max(pbl.easting, pbr.easting));
            northMin = Math.Min(Math.Min(ptl.northing, ptr.northing), Math.Min(pbl.northing, pbr.northing));
            northMax = Math.Max(Math.Max(ptl.northing, ptr.northing), Math.Max(pbl.northing, pbr.northing));


            /*

            float[] proj = new float[16];                           // For Grabbing The PROJECTION Matrix
            float[] modl = new float[16];                           // For Grabbing The MODELVIEW Matrix
            float[] clip = new float[16];                           // Result Of Concatenating PROJECTION and MODELVIEW

            GL.GetFloat(GetPName.ProjectionMatrix, proj);   // Grab The Current PROJECTION Matrix
            GL.GetFloat(GetPName.Modelview0MatrixExt, modl);   // Grab The Current MODELVIEW Matrix

            // Concatenate (Multiply) The Two Matricies
            clip[0] = modl[0] * proj[0] + modl[1] * proj[4] + modl[2] * proj[8] + modl[3] * proj[12];
            clip[1] = modl[0] * proj[1] + modl[1] * proj[5] + modl[2] * proj[9] + modl[3] * proj[13];
            clip[2] = modl[0] * proj[2] + modl[1] * proj[6] + modl[2] * proj[10] + modl[3] * proj[14];
            clip[3] = modl[0] * proj[3] + modl[1] * proj[7] + modl[2] * proj[11] + modl[3] * proj[15];

            clip[4] = modl[4] * proj[0] + modl[5] * proj[4] + modl[6] * proj[8] + modl[7] * proj[12];
            clip[5] = modl[4] * proj[1] + modl[5] * proj[5] + modl[6] * proj[9] + modl[7] * proj[13];
            clip[6] = modl[4] * proj[2] + modl[5] * proj[6] + modl[6] * proj[10] + modl[7] * proj[14];
            clip[7] = modl[4] * proj[3] + modl[5] * proj[7] + modl[6] * proj[11] + modl[7] * proj[15];

            clip[8] = modl[8] * proj[0] + modl[9] * proj[4] + modl[10] * proj[8] + modl[11] * proj[12];
            clip[9] = modl[8] * proj[1] + modl[9] * proj[5] + modl[10] * proj[9] + modl[11] * proj[13];
            clip[10] = modl[8] * proj[2] + modl[9] * proj[6] + modl[10] * proj[10] + modl[11] * proj[14];
            clip[11] = modl[8] * proj[3] + modl[9] * proj[7] + modl[10] * proj[11] + modl[11] * proj[15];

            clip[12] = modl[12] * proj[0] + modl[13] * proj[4] + modl[14] * proj[8] + modl[15] * proj[12];
            clip[13] = modl[12] * proj[1] + modl[13] * proj[5] + modl[14] * proj[9] + modl[15] * proj[13];
            clip[14] = modl[12] * proj[2] + modl[13] * proj[6] + modl[14] * proj[10] + modl[15] * proj[14];
            clip[15] = modl[12] * proj[3] + modl[13] * proj[7] + modl[14] * proj[11] + modl[15] * proj[15];


            // Extract the RIGHT clipping plane
            frustum[0] = clip[3] - clip[0];
            frustum[1] = clip[7] - clip[4];
            frustum[2] = clip[11] - clip[8];
            frustum[3] = clip[15] - clip[12];

            // Extract the LEFT clipping plane
            frustum[4] = clip[3] + clip[0];
            frustum[5] = clip[7] + clip[4];
            frustum[6] = clip[11] + clip[8];
            frustum[7] = clip[15] + clip[12];

            // Extract the FAR clipping plane
            frustum[8] = clip[3] - clip[2];
            frustum[9] = clip[7] - clip[6];
            frustum[10] = clip[11] - clip[10];
            frustum[11] = clip[15] - clip[14];


            // Extract the NEAR clipping plane.  This is last on purpose (see pointinfrustum() for reason)
            frustum[12] = clip[3] + clip[2];
            frustum[13] = clip[7] + clip[6];
            frustum[14] = clip[11] + clip[10];
            frustum[15] = clip[15] + clip[14];

            // Extract the BOTTOM clipping plane
            frustum[16] = clip[3] + clip[1];
            frustum[17] = clip[7] + clip[5];
            frustum[18] = clip[11] + clip[9];
            frustum[19] = clip[15] + clip[13];

            // Extract the TOP clipping plane
            frustum[20] = clip[3] - clip[1];
            frustum[21] = clip[7] - clip[5];
            frustum[22] = clip[11] - clip[9];
            frustum[23] = clip[15] - clip[13];
            */
        }

        //determine mins maxs of patches and whole field.
        public void CalculateMinMax()
        {
            btnABDraw.Visible = bnd.bndList.Count > 0;
            minFieldX = 9999999; minFieldY = 9999999;
            maxFieldX = -9999999; maxFieldY = -9999999;


            //min max of the boundary
            //min max of the boundary
            if (bnd.bndList.Count > 0)
            {
                int bndCnt = bnd.bndList[0].fenceLine.points.Count;
                for (int i = 0; i < bndCnt; i++)
                {
                    double x = bnd.bndList[0].fenceLine.points[i].easting;
                    double y = bnd.bndList[0].fenceLine.points[i].northing;

                    //also tally the max/min of field x and z
                    if (minFieldX > x) minFieldX = x;
                    if (maxFieldX < x) maxFieldX = x;
                    if (minFieldY > y) minFieldY = y;
                    if (maxFieldY < y) maxFieldY = y;
                }

            }
            else
            {
                //for every new chunk of patch
                foreach (Polyline2 triList in patchList)
                {
                    int count2 = triList.points.Count;
                    for (int i = 2; i < count2; i += 3)
                    {
                        double x = triList.points[i].easting;
                        double y = triList.points[i].northing;

                        //also tally the max/min of field x and z
                        if (minFieldX > x) minFieldX = x;
                        if (maxFieldX < x) maxFieldX = x;
                        if (minFieldY > y) minFieldY = y;
                        if (maxFieldY < y) maxFieldY = y;
                    }
                }
            }

            if (maxFieldX == -9999999 || minFieldX == 9999999 || maxFieldY == -9999999 || minFieldY == 9999999)
            {
                maxFieldX = 0; minFieldX = 0; maxFieldY = 0; minFieldY = 0; maxFieldDistance = 1500;
            }
            else
            {
                //the largest distancew across field
                double dist = Math.Abs(minFieldX - maxFieldX);
                double dist2 = Math.Abs(minFieldY - maxFieldY);

                maxCrossFieldLength = Math.Sqrt(dist * dist + dist2 * dist2) * 1.05;

                if (dist > dist2) maxFieldDistance = (dist);
                else maxFieldDistance = (dist2);

                maxFieldDistance += 100;

                fieldCenterX = (maxFieldX + minFieldX) / 2.0;
                fieldCenterY = (maxFieldY + minFieldY) / 2.0;
            }

            bnd.UpdateFieldBoundaryGUIAreas();
        }

        public Vector4 NDCToWorld(Vector4 ndc_corner, Matrix4 inv_view, Matrix4 inv_proj)
        {
            var view_corner_h = Vector4.Transform(ndc_corner, inv_view);
            var view_corner = view_corner_h * 1 / view_corner_h[3];
            return Vector4.Transform(view_corner, inv_proj);
        }

        private void DrawFieldText()
        {
            StringBuilder sb = new StringBuilder();
            if (bnd.bndList.Count > 0)
            {
                sb.Clear();
                sb.Append(((bnd.workedAreaTotal - bnd.actualAreaCovered) * glm.m2ToUser).ToString("0.000"));
                sb.Append(glm.unitsHaAc + " ");
                sb.Append(bnd.overlapPercent.ToString("0.00"));
                sb.Append("%  ");
                sb.Append((bnd.areaBoundaryOuterLessInner * glm.m2ToUser).ToString("0.00"));
                sb.Append("-");
                sb.Append((bnd.actualAreaCovered * glm.m2ToUser).ToString("0.00"));
                sb.Append(" = ");
                sb.Append(((bnd.areaBoundaryOuterLessInner - bnd.actualAreaCovered) * glm.m2ToUser).ToString("0.00"));
                sb.Append(glm.unitsHaAc + "  ");
                sb.Append(bnd.TimeTillFinished);
                GL.Color3(0.95, 0.95, 0.95);
                font.DrawText(-sb.Length * 7, oglMain.Height - 32, sb.ToString());
            }
            else
            {
                sb.Clear();
                //sb.Append("Overlap ");
                sb.Append(bnd.overlapPercent.ToString("0.000"));
                sb.Append("%   ");
                sb.Append((bnd.actualAreaCovered * glm.m2ToUser).ToString("0.000"));
                sb.Append(glm.unitsHaAc);
                GL.Color3(0.95, 0.95, 0.95);
                font.DrawText(0, oglMain.Height - 32, sb.ToString());
            }
        }
    }
}
