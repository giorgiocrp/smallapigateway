namespace ApiAuth.Model
{
    public class AuthenticationToken
    {
        private string _tokenString;
        private int _totalSeconds;

        public AuthenticationToken(string tokenString, int totalSeconds)
        {
            _tokenString=tokenString;
            _totalSeconds=totalSeconds;
        }

        public string Token { get { return _tokenString; } set { value=_tokenString; } }
        public int ExpiresIn { get { return _totalSeconds; } set { value=_totalSeconds; } }
    }
}
