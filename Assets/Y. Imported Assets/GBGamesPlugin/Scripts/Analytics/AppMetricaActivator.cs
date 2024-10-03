using Io.AppMetrica;
using UnityEngine;

public static class AppMetricaActivator {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate() {
        AppMetrica.Activate(new AppMetricaConfig("158efc2a-53d6-4353-8813-ef5f25a4b30a") {
            FirstActivationAsUpdate = !IsFirstLaunch(),
            LocationTracking = false,
            SessionTimeout = 300
        });
    }

    private static bool IsFirstLaunch() {
        // Implement logic to detect whether the app is opening for the first time.
        // For example, you can check for files (settings, databases, and so on),
        // which the app creates on its first launch.
        return true;
    }
}