using RoR2;
using UnityEngine;

internal enum RegigigasCameraParams
{
    DEFAULT,
    AIM,
    CHARGE,
    EMOTE
}

namespace RegigigasMod.Modules
{
    internal static class CameraParams
    {
        internal static CharacterCameraParamsData defaultCameraParams;
        internal static CharacterCameraParamsData aimCameraParams;
        internal static CharacterCameraParamsData chargeCameraParams;
        internal static CharacterCameraParamsData emoteCameraParams;

        internal static void InitializeParams()
        {
            defaultCameraParams = NewCameraParams("ccpRegigigas", 70f, 1.37f, new Vector3(0f, 10f, -30f));
            aimCameraParams = NewCameraParams("ccpRegigigasAim", 70f, 1.37f, new Vector3(0f, 12f, -28f));
            chargeCameraParams = NewCameraParams("ccpRegigigasCharge", 70f, 1.37f, new Vector3(0f, 3f, -35f));
            emoteCameraParams = NewCameraParams("ccpRegigigasEmote", 70f, 1.37f, new Vector3(0f, 5f, -40f));
        }

        private static CharacterCameraParamsData NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition)
        {
            return NewCameraParams(name, pitch, pivotVerticalOffset, standardPosition, 0.1f);
        }

        private static CharacterCameraParamsData NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 idealPosition, float wallCushion)
        {
            CharacterCameraParamsData newParams = new CharacterCameraParamsData();

            newParams.maxPitch = pitch;
            newParams.minPitch = -pitch;
            newParams.pivotVerticalOffset = pivotVerticalOffset;
            newParams.idealLocalCameraPos = idealPosition;
            newParams.wallCushion = wallCushion;

            return newParams;
        }

        internal static CameraTargetParams.CameraParamsOverrideHandle OverrideCameraParams(CameraTargetParams camParams, RegigigasCameraParams camera, float transitionDuration = 0.5f)
        {
            CharacterCameraParamsData paramsData = GetNewParams(camera);

            CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
            {
                cameraParamsData = paramsData,
                priority = 0,
            };

            return camParams.AddParamsOverride(request, transitionDuration);
        }

        internal static CharacterCameraParams CreateCameraParamsWithData(RegigigasCameraParams camera)
        {

            CharacterCameraParams newPaladinCameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();

            newPaladinCameraParams.name = camera.ToString().ToLower() + "Params";

            newPaladinCameraParams.data = GetNewParams(camera);

            return newPaladinCameraParams;
        }

        internal static CharacterCameraParamsData GetNewParams(RegigigasCameraParams camera)
        {
            CharacterCameraParamsData paramsData = defaultCameraParams;

            switch (camera)
            {

                default:
                case RegigigasCameraParams.DEFAULT:
                    paramsData = defaultCameraParams;
                    break;
                case RegigigasCameraParams.AIM:
                    paramsData = aimCameraParams;
                    break;
                case RegigigasCameraParams.CHARGE:
                    paramsData = chargeCameraParams;
                    break;
                case RegigigasCameraParams.EMOTE:
                    paramsData = emoteCameraParams;
                    break;
            }

            return paramsData;
        }
    }
}