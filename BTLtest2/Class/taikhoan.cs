using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    public class taikhoan
    {
        public string usename { get; set; }
        public string password { get; set; }
        public int phanquyen { get; set; }
        public taikhoan(string usename, string password, int phanquyen)
        {
            this.usename = usename;
            this.password = password;
            this.phanquyen = phanquyen;
        }
        public taikhoan()
        {
            this.usename = string.Empty;
            this.password = string.Empty;
            this.phanquyen = 0;
        }
    }
}
