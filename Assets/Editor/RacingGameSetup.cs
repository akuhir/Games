using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;
using System.IO;

public static class RacingGameSetup
{
    const string ScenePath = "Assets/Scenes/Main.unity";

    [MenuItem("Solograph Racing/Create Demo Scene")]
    public static void CreateDemoScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.45f,0.45f,0.45f);

        var light = new GameObject("Sun");
        var dl = light.AddComponent<Light>(); dl.type=LightType.Directional; dl.intensity=1.2f;
        light.transform.rotation=Quaternion.Euler(50,-30,0);

        // Ground
        CreateCube("Ground", new Vector3(0,-0.6f,0), new Vector3(70,1,50), new Color(0.12f,0.18f,0.12f));
        // Track strips: a simple rectangular circuit
        CreateCube("RoadNorth", new Vector3(0,0,15), new Vector3(60,0.25f,8), new Color(0.08f,0.08f,0.08f));
        CreateCube("RoadSouth", new Vector3(0,0,-15), new Vector3(60,0.25f,8), new Color(0.08f,0.08f,0.08f));
        CreateCube("RoadEast", new Vector3(26,0,0), new Vector3(8,0.25f,38), new Color(0.08f,0.08f,0.08f));
        CreateCube("RoadWest", new Vector3(-26,0,0), new Vector3(8,0.25f,38), new Color(0.08f,0.08f,0.08f));

        // Player car
        var car = CreateCube("Car", new Vector3(-26,0.8f,-8), new Vector3(2.2f,0.9f,4.2f), new Color(0.65f,0.05f,0.05f));
        var rb=car.AddComponent<Rigidbody>(); rb.mass=1100; rb.linearDamping=0.35f; rb.angularDamping=2.5f; rb.interpolation=RigidbodyInterpolation.Interpolate;
        car.AddComponent<CarController>();
        car.transform.rotation=Quaternion.Euler(0,0,0);

        // Camera
        var camObj=new GameObject("Main Camera"); camObj.tag="MainCamera";
        var cam=camObj.AddComponent<Camera>(); cam.fieldOfView=65;
        var follow=camObj.AddComponent<FollowCamera>(); follow.target=car.transform;
        camObj.transform.position=car.transform.position-car.transform.forward*8+Vector3.up*4;

        // Race manager
        var rm=new GameObject("RaceManager"); var manager=rm.AddComponent<RaceManager>(); manager.totalCheckpoints=6; manager.lapsToWin=3;

        Vector3[] cps={
            new Vector3(-26,0.2f,-1), new Vector3(-26,0.2f,15), new Vector3(0,0.2f,15),
            new Vector3(26,0.2f,15), new Vector3(26,0.2f,-1), new Vector3(0,0.2f,-15)
        };
        for(int i=0;i<cps.Length;i++){
            var cp=CreateCube("Checkpoint_"+i,cps[i],new Vector3(5,2,1),new Color(0.2f,0.8f,0.2f,0.18f));
            var col=cp.GetComponent<BoxCollider>(); col.isTrigger=true;
            var c=cp.AddComponent<Checkpoint>(); c.checkpointIndex=i;
        }

        // Event system
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        var canvasObj=new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas=canvasObj.GetComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        var scaler=canvasObj.GetComponent<CanvasScaler>(); scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution=new Vector2(1920,1080);

        // HUD
        var hudObj=new GameObject("HUD"); hudObj.transform.SetParent(canvasObj.transform,false);
        var hud=hudObj.AddComponent<RaceHUD>();
        hud.lapText=MakeText(hudObj.transform,"LapText",new Vector2(1700,980),new Vector2(360,70),"Lap: 0 / 3",42,TextAnchor.MiddleCenter);
        hud.timeText=MakeText(hudObj.transform,"TimeText",new Vector2(1700,900),new Vector2(360,70),"Time: 00:00.00",42,TextAnchor.MiddleCenter);
        var finish=MakePanel(hudObj.transform,"FinishPanel",new Vector2(960,540),new Vector2(700,420),new Color(0,0,0,0.82f));
        hud.finishPanel=finish.gameObject;
        hud.resultText=MakeText(finish.transform,"Result",new Vector2(0,55),new Vector2(650,170),"Race Complete!",54,TextAnchor.MiddleCenter);
        var restart=MakeButton(finish.transform,"Restart",new Vector2(0,-100),new Vector2(300,90),"RESTART");
        restart.onClick.AddListener(manager.RestartRace);

        // Mobile controls
        var controls=new GameObject("MobileControls"); controls.transform.SetParent(canvasObj.transform,false); var mc=controls.AddComponent<MobileControls>(); mc.car=car.GetComponent<CarController>();
        AddHoldButton(controls.transform,"LEFT",new Vector2(180,150),new Vector2(190,140),()=>mc.LeftDown(),()=>mc.SteeringUp());
        AddHoldButton(controls.transform,"RIGHT",new Vector2(400,150),new Vector2(190,140),()=>mc.RightDown(),()=>mc.SteeringUp());
        AddHoldButton(controls.transform,"BRAKE",new Vector2(1540,150),new Vector2(230,140),()=>mc.BrakeDown(),()=>mc.BrakeUp());
        AddHoldButton(controls.transform,"GAS",new Vector2(1790,150),new Vector2(230,140),()=>mc.AccelerateDown(),()=>mc.GasUp());

        EditorSceneManager.SaveScene(scene,ScenePath);
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        Selection.activeObject=car;
        Debug.Log("RacingGame demo scene created: "+ScenePath);
    }

    [MenuItem("Solograph Racing/Build Android APK")]
    public static void BuildAndroid()
    {
        if(!File.Exists(ScenePath)) CreateDemoScene();
        EditorUserBuildSettings.buildAppBundle=false;
        EditorUserBuildSettings.androidBuildSystem=AndroidBuildSystem.Gradle;
        PlayerSettings.applicationIdentifier="com.solograph.racinggame";
        PlayerSettings.productName="Solograph Racing";
        PlayerSettings.companyName="Solograph";
        Directory.CreateDirectory("build/Android");
        var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{
            scenes=new[]{ScenePath}, locationPathName="build/Android/SolographRacing.apk", target=BuildTarget.Android, options=BuildOptions.None
        });
        if(report.summary.result!=BuildResult.Succeeded) throw new System.Exception("Android build failed: "+report.summary.result);
        Debug.Log("APK built: build/Android/SolographRacing.apk");
    }

    static GameObject CreateCube(string name,Vector3 pos,Vector3 scale,Color color){
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube); go.name=name; go.transform.position=pos; go.transform.localScale=scale;
        var mat=new Material(Shader.Find("Universal Render Pipeline/Lit")); if(mat.shader==null) mat=new Material(Shader.Find("Standard")); mat.color=color; go.GetComponent<Renderer>().sharedMaterial=mat; return go;
    }
    static TMP_Text MakeText(Transform parent,string name,Vector2 pos,Vector2 size,string text,float fs,TextAnchor anchor){
        var go=new GameObject(name); go.transform.SetParent(parent,false); var rt=go.AddComponent<RectTransform>(); rt.anchoredPosition=pos; rt.sizeDelta=size; var t=go.AddComponent<TextMeshProUGUI>(); t.text=text; t.fontSize=fs; t.alignment=anchor==TextAnchor.MiddleCenter?TextAlignmentOptions.Center:TextAlignmentOptions.Left; return t;
    }
    static Image MakePanel(Transform parent,string name,Vector2 pos,Vector2 size,Color color){
        var go=new GameObject(name); go.transform.SetParent(parent,false); var rt=go.AddComponent<RectTransform>(); rt.anchoredPosition=pos; rt.sizeDelta=size; var im=go.AddComponent<Image>(); im.color=color; return im;
    }
    static Button MakeButton(Transform parent,string name,Vector2 pos,Vector2 size,string label){
        var go=new GameObject(name); go.transform.SetParent(parent,false); var rt=go.AddComponent<RectTransform>(); rt.anchoredPosition=pos; rt.sizeDelta=size; var im=go.AddComponent<Image>(); im.color=new Color(.15f,.15f,.15f,.9f); var b=go.AddComponent<Button>(); var txt=MakeText(go.transform,"Label",Vector2.zero,size,label,34,TextAnchor.MiddleCenter); return b;
    }
    static void AddHoldButton(Transform parent,string label,Vector2 pos,Vector2 size,UnityEngine.Events.UnityAction down,UnityEngine.Events.UnityAction up){
        var go=MakeButton(parent,label,pos,size,label).gameObject; var trigger=go.AddComponent<EventTrigger>();
        var p=new EventTrigger.Entry{eventID=EventTriggerType.PointerDown}; p.callback.AddListener(_=>down()); trigger.triggers.Add(p);
        var r=new EventTrigger.Entry{eventID=EventTriggerType.PointerUp}; r.callback.AddListener(_=>up()); trigger.triggers.Add(r);
        var e=new EventTrigger.Entry{eventID=EventTriggerType.PointerExit}; e.callback.AddListener(_=>up()); trigger.triggers.Add(e);
    }
}
