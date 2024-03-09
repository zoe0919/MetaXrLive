using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainUIScript : MonoBehaviour
{
    bool inMenu;
    private Text sliderText;
    private GameObject moviePlayer = null;
    private MoviePlayerSample moviePlayerSample = null;

    void Start()
    {
        Logger.Log("MainUIScript start");
        // DebugUIBuilder.instance.AddButton("Login", LogButtonPressed);
        DebugUIBuilder.instance.AddLabel("Channel List");
        // var sliderPrefab = DebugUIBuilder.instance.AddSlider("Slider", 1.0f, 10.0f, SliderPressed, true);
        // var textElementsInSlider = sliderPrefab.GetComponentsInChildren<Text>();
        // Assert.AreEqual(textElementsInSlider.Length, 2,
        //     "Slider prefab format requires 2 text components (label + value)");
        // sliderText = textElementsInSlider[1];
        // Assert.IsNotNull(sliderText, "No text component on slider prefab");
        // sliderText.text = sliderPrefab.GetComponentInChildren<Slider>().value.ToString();
        // DebugUIBuilder.instance.AddDivider();
        // DebugUIBuilder.instance.AddToggle("Toggle", TogglePressed);
        DebugUIBuilder.instance.AddRadio("dog1", "group", delegate (Toggle t) { RadioPressed("dog1", "group", t); });
        DebugUIBuilder.instance.AddRadio("dog2", "group", delegate (Toggle t) { RadioPressed("dog2", "group", t); });
        DebugUIBuilder.instance.AddRadio("dog3", "group", delegate (Toggle t) { RadioPressed("dog3", "group", t); });
        DebugUIBuilder.instance.AddRadio("dog4", "group", delegate (Toggle t) { RadioPressed("dog4", "group", t); });
        DebugUIBuilder.instance.AddRadio("dog5", "group", delegate (Toggle t) { RadioPressed("dog5", "group", t); });
        // DebugUIBuilder.instance.AddLabel("Secondary Tab", 1);
        // DebugUIBuilder.instance.AddDivider(1);
        // DebugUIBuilder.instance.AddRadio("Side Radio 1", "group2",
        //     delegate (Toggle t) { RadioPressed("Side Radio 1", "group2", t); }, DebugUIBuilder.DEBUG_PANE_RIGHT);
        // DebugUIBuilder.instance.AddRadio("Side Radio 2", "group2",
        //     delegate (Toggle t) { RadioPressed("Side Radio 2", "group2", t); }, DebugUIBuilder.DEBUG_PANE_RIGHT);

        DebugUIBuilder.instance.Show();
        inMenu = true;

        moviePlayer = GameObject.Find("MoviePlayer");
        if (moviePlayer != null) 
        {
            moviePlayerSample = moviePlayer.GetComponent<MoviePlayerSample>();
        }
    }

    // public void TogglePressed(Toggle t)
    // {
    //     Debug.Log("Toggle pressed. Is on? " + t.isOn);
    // }

    public void RadioPressed(string radioLabel, string group, Toggle t)
    {
        Logger.Log("Radio value changed: " + radioLabel + ", from group " + group + ". New value: " + t.isOn);
        if (moviePlayerSample == null)
        {
            Logger.LogWarning("moviePlayerSample == null!");
            return;
        }
        switch(radioLabel) 
        {
            case "dog1":
                moviePlayerSample.Stop();
                moviePlayerSample.Play("Assets/Main/Videos/" + "dog1.mp4", null);
                break;
            case "dog2":
                moviePlayerSample.Stop();
                moviePlayerSample.Play("Assets/Main/Videos/" + "dog2.mp4", null);
                break;
            case "dog3":
                moviePlayerSample.Stop();
                moviePlayerSample.Play("Assets/Main/Videos/" + "dog3.mp4", null);
                break;
            case "dog4":
                moviePlayerSample.Stop();
                moviePlayerSample.Play("Assets/Main/Videos/" + "dog4.mp4", null);
                break;
            case "dog5":
                moviePlayerSample.Stop();
                moviePlayerSample.Play("Assets/Main/Videos/" + "dog5.mp4", null);
                break;
            default:
                break;
        }
    }

    // public void SliderPressed(float f)
    // {
    //     Debug.Log("Slider: " + f);
    //     sliderText.text = f.ToString();
    // }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            if (inMenu) DebugUIBuilder.instance.Hide();
            else DebugUIBuilder.instance.Show();
            inMenu = !inMenu;
        }
    }

    // void LogButtonPressed()
    // {
    //     Logger.Log("Button pressed");
    // }
}
