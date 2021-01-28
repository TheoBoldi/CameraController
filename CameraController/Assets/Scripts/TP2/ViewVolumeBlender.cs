using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private static ViewVolumeBlender instance = null;

    public static ViewVolumeBlender Instance
    {
        get
        {
            return instance;
        }
    }

    private List<AViewVolume> activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> volumesPerViews = new Dictionary<AView, List<AViewVolume>>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void CheckPriorities()
    {
        int highestPriority = -1;

        for (int i = 0; i < activeViewVolumes.Count; i++)
        {
            activeViewVolumes[i].view.weight = 0;
            if (activeViewVolumes[i].priority > highestPriority)
                highestPriority = activeViewVolumes[i].priority;
        }

        for (int i = 0; i < activeViewVolumes.Count; i++)
        {
            if (activeViewVolumes[i].priority < highestPriority)
                continue;
            activeViewVolumes[i].view.weight = 1f;
        }
    }

    public void AddVolume(AViewVolume aViewVolume)
    {
        activeViewVolumes.Add(aViewVolume);
        if (!volumesPerViews.ContainsKey(aViewVolume.view))
        {
            List<AViewVolume> list = new List<AViewVolume>();
            list.Add(aViewVolume);
            volumesPerViews.Add(aViewVolume.view, list);

            aViewVolume.view.SetActive(true);
        }
        else
        {
            if (!volumesPerViews[aViewVolume.view].Contains(aViewVolume))
                volumesPerViews[aViewVolume.view].Add(aViewVolume);
        }
        CheckPriorities();
    }

    public void RemoveVolume(AViewVolume aViewVolume)
    {
        activeViewVolumes.Remove(aViewVolume);
        volumesPerViews[aViewVolume.view].Remove(aViewVolume);
        if (volumesPerViews[aViewVolume.view].Count == 0)
        {
            volumesPerViews.Remove(aViewVolume.view);
            aViewVolume.view.SetActive(false);
        }
        CheckPriorities();
    }

    private void OnGUI()
    {
        foreach (AViewVolume aViewVolume in activeViewVolumes)
        {
            GUILayout.Label(aViewVolume.name);
        }
    }
}
