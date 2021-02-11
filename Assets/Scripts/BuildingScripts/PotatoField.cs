using System;

public class PotatoField : Building
{
    // would hold your ui input objects and w/e was in `Objects`
    private BuildingUI _ui;
    private Building _building;
    private uint _buildingId;

    private void Awake()
    {
        _ui = GetComponent<BuildingUI>();
    }

    private void Start()
    {
        _buildingId = BuildingManager.Register(_building);
        _ui.RegisterEvents(_building);

        Resource[] resources = BuildingManager.RequestResourceCache<PotatoField>();
        for (int i = 0; i < resources.Length; i++)
        {
            _building.RegisterResource(resources[i].type, resources[i].storedAmount, resources[i].cost);
        }
    }

    private void OnDestroy()
    {
        BuildingManager.Unregister(_buildingId);
        _ui.OnBuildClicked -= _building.Build;
    }
}



