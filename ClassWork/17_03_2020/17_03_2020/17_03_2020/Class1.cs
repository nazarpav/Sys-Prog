using System;

namespace _17_03_2020
{
    public class Tank
    {
        private string _name;//ім'я
        private int _ammunition;//боєкомплект
        private int _armor;//рівень броні
        private int _maneuverability;//рівень маневреності
        private static Random rnd;
        static Tank()
        {
            rnd = new Random();
        }
        public Tank(string name)
        {
            _name = name;
            _ammunition = rnd.Next(0, 101);
            _armor = rnd.Next(0, 101);
            _maneuverability = rnd.Next(0, 101);
        }
        public static bool operator ^(Tank tank1, Tank tank2)
        {
            if((tank1._maneuverability>tank2._maneuverability&&tank1._armor>tank2._armor)||
                (tank1._maneuverability > tank2._maneuverability && tank1._ammunition > tank2._ammunition)||
                (tank1._armor > tank2._armor && tank1._ammunition > tank2._ammunition))
            {
                return true;
            }
            return false;
        }
        public override string ToString()
        {
            return "Name: "+_name+ " |Ammunition: "+ _ammunition+ " |Armor: "+ _armor+ " |Maneuverability: "+ _maneuverability;
        }
    }
}
