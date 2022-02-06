namespace AgOpenGPS
{
    public partial class CBoundary
    {
        public bool isHeadlandOn;
        public bool isToolInHeadland,
            isToolOuterPointsInHeadland, isSectionControlledByHeadland;

        public void SetHydPosition()
        {
            if (mf.vehicle.isHydLiftOn && mf.pn.speed > 0.2 && mf.autoBtnState == FormGPS.btnStates.Auto)
            {
                if (isToolInHeadland)
                {
                    mf.p_239.pgn[mf.p_239.hydLift] = 2;
                    if (mf.sounds.isHydLiftChange != isToolInHeadland)
                    {
                        if (mf.sounds.isHydLiftSoundOn) CSound.sndHydLiftUp.Play();
                        mf.sounds.isHydLiftChange = isToolInHeadland;
                    }
                }
                else
                {
                    mf.p_239.pgn[mf.p_239.hydLift] = 1;
                    if (mf.sounds.isHydLiftChange != isToolInHeadland)
                    {
                        if (mf.sounds.isHydLiftSoundOn) CSound.sndHydLiftDn.Play();
                        mf.sounds.isHydLiftChange = isToolInHeadland;
                    }
                }
            }
        }
    }
}