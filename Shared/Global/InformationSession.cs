namespace Shared.Global
{
    public class InformationSession
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string IdCard { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public int Role { get; set; }

        public bool ContainsRol(int SearchRole) => ContainsRol(new int[] { SearchRole });

        public bool ContainsRol(string? SearchRole)
        {
            return ContainsRol(SearchRole.ReplaceIfNullOrEmpty("0").Split(',', ';', ' '));
        }

        public bool ContainsRol(IEnumerable<int> list)
        {
            return list.Any(searchRole => Role == searchRole);
        }

        public bool ContainsRol(IEnumerable<string> list)
        {
            if (list != null && list.Any())
                return ContainsRol(list.Select(x => x.ConvertObjectToInt()));
            return false;
        }

        public void Personificar(int userId, int idRol, string userName)
        {
            this.UserId = userId;
            this.Role = idRol;
            this.UserName= userName;
        }
    }
}
