namespace SimpleSQL.Demos
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UpdateRecord : MonoBehaviour
    {
        private int playerId;

        public Text recordText;
        public Action<int> editButtonClicked;

        public void SetRecord(int playerId, string recordText)
        {
            this.playerId = playerId;

            this.recordText.text = recordText;
        }

        public void EditButtonClicked()
        {
            if (editButtonClicked != null)
            {
                editButtonClicked(playerId);
            }
        }
    }
}
