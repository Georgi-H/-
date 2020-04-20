using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Windows.Forms;

namespace Snake3
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable();
        //new instance for hashtable class
        //the class is used to optimize the keys

        public static bool KeyPress(Keys key)
        {
            //will return a key back to the classa
            if (keyTable[key] == null)
            {
                // if hastable is empty we return false
                return false;
            }
            return (bool)keyTable[key];
        }
        public static void changeState(Keys key, bool state)
        {
            //function changes state of keys and player
            // it has 2 arguments: keys and state
            keyTable[key] = state;
        }
    }
}
