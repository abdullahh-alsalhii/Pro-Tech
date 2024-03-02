using ProـTech_Web.DataBase;
using ProـTech_Web.Global;

namespace ProـTech_Web.Interfaces
{

    public interface Dto_Void_Funcs
    {
        public Code_Res Update();
        public Code_Res Create();
        public Code_Res Delete();
    }
    public interface Dto_Value_Funcs<OneModel, ListModel>
    {
        public Code_Res_With_Data<OneModel> GetById(int Id);
        public Code_Res_With_Data<ListModel> GetList(GetFromTo fromTo);
    }

}
