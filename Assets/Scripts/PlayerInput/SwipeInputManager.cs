using System;

using UnityEngine;


    public class SwipeInputManager : IInputManager
    {
        public override event Action<Vector2> InputEvent;
        
        private Vector2 firstPressPos;
        private Vector2 secondPressPos;
        private Vector2 currentSwipe;

        private float leftXBorder = -0.5f;
        private float rightXBorder = 0.5f;

        private float minLen = 80;

        void Update()
        {
            MoveAction();
        }
        
        public void MoveAction()
        {
            Swipe();
        }
        

        private void Swipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                if (currentSwipe.magnitude >= minLen)
                {
                    currentSwipe.Normalize();
                    
                    if (currentSwipe.y > 0 && currentSwipe.x > leftXBorder && currentSwipe.x < rightXBorder)
                    {
                        InputEvent?.Invoke(Vector2.up);
                    }
                    else if (currentSwipe.y < 0 && currentSwipe.x > leftXBorder && currentSwipe.x < rightXBorder)
                    {
                        InputEvent?.Invoke(Vector2.down);
                    }
                    else if (currentSwipe.x < 0 && currentSwipe.y > leftXBorder && currentSwipe.y < rightXBorder)
                    {
                        InputEvent?.Invoke(Vector2.left);
                    }
                    else if (currentSwipe.x > 0 && currentSwipe.y > leftXBorder && currentSwipe.y < rightXBorder)
                    {
                        InputEvent?.Invoke(Vector2.right);
                    }
                }
            }
        }
    }

    
