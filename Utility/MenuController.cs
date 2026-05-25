using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private UIDocument _doc;
    private Button _newGame;
    private Button _loadGame;
    private Button [] _backButtons = new Button[4];

    private GroupBox [] _menus = new GroupBox[4];


    private void Awake() {
        //Get all components
        _doc = GetComponent<UIDocument>();

        _menus[0] = _doc.rootVisualElement.Q<GroupBox>("main_menu");
        _menus[1] = _doc.rootVisualElement.Q<GroupBox>("load_game_menu");
        _menus[2] = _doc.rootVisualElement.Q<GroupBox>("options_menu");
        _menus[3] = _doc.rootVisualElement.Q<GroupBox>("extras_menu");

        for(int i = 0; i < _menus.Length; i++) {
            if(i == 0) {
                _menus[i].style.display = DisplayStyle.Flex;
                continue;
            }
            _menus[i].style.display = DisplayStyle.None;
        }

        _newGame = _doc.rootVisualElement.Q<Button>("new_game");
        _newGame.clicked += () => { SceneManager.LoadScene("Prison1"); };

        _loadGame = _doc.rootVisualElement.Q<Button>("load_game");
        _loadGame.clicked += () => {
            for (int i = 0; i < _menus.Length; i++) {
                if (i == 1) {
                    _menus[i].style.display = DisplayStyle.Flex;
                    continue;
                }
                _menus[i].style.display = DisplayStyle.None;
            }
        };

    }
}
