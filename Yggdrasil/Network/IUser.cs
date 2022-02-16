using Yggdrasil.Network;
using System;

namespace Yggdrasil.Network
{
	/// <summary>
	/// Description of IUser.
	/// </summary>
	public interface IUser
    {
        IClient Client { get; set; }
    }
}
