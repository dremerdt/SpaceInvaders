using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders.Menu {
    class MenuItem {
        public string Name { get; set; }
        public string SName { get; set; }
        public bool Active { get; set; }

        public event EventHandler Click;

        public MenuItem(string name) {
            Name = name;
            Active = true;
        }
        public void onClick() {
            if (Click != null) {
                if (SName != null)
                    swichNames();
                Click(this, null);               
            }
        }
        private void swichNames() {
            string temp = Name;
            Name = SName;
            SName = temp;
        }
    }
}
