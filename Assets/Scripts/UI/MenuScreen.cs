using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web;

namespace UI
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] private Toggle joystickToggle;
        [SerializeField] private Toggle swipesToggle;
        [SerializeField] private Toggle gyroToggle;
        [SerializeField] private TextMeshProUGUI adviceText;
        [Header("Web")] 
        [SerializeField] private AdviceWebManager webManager; 

        public event Action ToggleSwitched;

        private void Start()
        {
            webManager.MessageReceivedEvent += OnMessageReceive;
            joystickToggle.onValueChanged.AddListener(SwitchToJoystick);
            swipesToggle.onValueChanged.AddListener(SwitchToSwipes);
            gyroToggle.onValueChanged.AddListener(SwitchToGyroscope);
            
            var input = PlayerPrefs.GetString("Input");

            switch (input)
            {
                case "Joystick":
                    joystickToggle.isOn = true;
                    break;
                case "Swipe":
                    swipesToggle.isOn = true;
                    break;
                case "Gyroscope":
                    gyroToggle.isOn = true;
                    break;
            }
        }

        private void OnMessageReceive(Message message)
        {
            adviceText.text = message.slip.advice;
        }

        private void SwitchToJoystick(bool isOn)
        {
            if (isOn)
            {
                SwitchInputByString("Joystick");
            }
        }
        private void SwitchToSwipes(bool isOn)
        {
            if (isOn)
            {
                SwitchInputByString("Swipe");
            }
        }
        private void SwitchToGyroscope(bool isOn)
        {
            if (isOn)
            {
                SwitchInputByString("Gyroscope");
            }
        }
        private void SwitchInputByString(string str)
        {
            PlayerPrefs.SetString("Input",str);
            ToggleSwitched?.Invoke();
           
        }

        private void OnEnable()
        {
            webManager.RequestMessage();
        }
    }
}