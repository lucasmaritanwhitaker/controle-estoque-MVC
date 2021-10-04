namespace ControleEstoque.Web.Helpers
{
    public static class ConnectionHelper
    {
        //private const string Server = @"DESKTOP-SDT4Q2C\SQLEXPRESS";
        private const string Server = @"SMN_RAFAELMORAI\SQLEXPRESS";
        private const string UserId = "admin";
        private const string Password = "123";
        private const string BancoName = "controle-estoque";

        public static string ConnectionString = $@"Data Source={Server};Initial Catalog={BancoName};User Id={UserId};Password={Password}";
    }
}