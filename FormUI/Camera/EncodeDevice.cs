namespace FormUI.Camera
{
    public class EncodeDevice
    {
        public virtual int ID { get; set; }

        public virtual string Name { get; set; }

        public virtual int LoginMode { get; set; }

        public virtual string LoginAdress { get; set; }

        public virtual string LoginPort { get; set; }

        public virtual string UserName { get; set; }

        public virtual string UserPwd { get; set; }

        public virtual string SN { get; set; }
    }
}