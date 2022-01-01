using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IHandler
{
    void MessageReceive(SocketModel model);
}

