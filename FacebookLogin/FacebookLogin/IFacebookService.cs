using System;
using System.Threading.Tasks;

namespace FacebookLogin
{
    public interface IFacebookService
    {
        Task<LoginResult> Login();
    }
}