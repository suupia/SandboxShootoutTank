// public class SelectUnitUI : MonoBehaviour
// {
//     [SerializeField] GameObject selectUnitUIObj;
//     [SerializeField] Transform selectUnitButtonsParent;
//     PlayerSpawner playerSpawner;
//
//     void Start()
//     {
//         playerSpawner = new PlayerSpawner();
//
//         var buttons = selectUnitButtonsParent.GetComponentsInChildren<Button>();
//         for (int i = 0; i < buttons.Length; i++)
//         {
//             int index = i;
//             buttons[i].onClick.AddListener(() => OnClick(index));
//         }
//     }
//
//     void OnClick(int index)
//     {
//         Debug.Log($"number:{index}");
//         var unitType = (PlayerController.UnitType)index;
//         playerSpawner.SetUnitType((PlayerController.UnitType)index);
//
//         playerSpawner.SpawnPlayer();
//
//         CloseUI();
//     }
//
//     void CloseUI()
//     {
//         selectUnitUIObj.gameObject.SetActive(false);
//     }
// }

