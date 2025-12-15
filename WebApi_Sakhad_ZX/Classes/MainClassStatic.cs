namespace WebApi_Sakhad_ZX
{
    public static class MainClassStatic
    {
        public static List<SakhadCenter> sakhadCenters = new List<SakhadCenter>();
        public static List<InitilizerCenter> InitCenter = new List<InitilizerCenter>();

        private static string Workstationid = "176.123.64.2";
        private static string ClientSecret = "MedicalSciencesShirazSecret";
        private static string ClientId = "MedicalSciencesShiraz";

        static MainClassStatic()
        {
            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122089,//داروخانه
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });

            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122090,//آزمایشگاه
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });

            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122091,//تصویر برداری
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });

            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122092,//تونبخشی
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });

            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122093,//خدمات پزشکی
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });

            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122094,//مطب دندانپزشکی
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });

            InitCenter.Add(new InitilizerCenter()
            {
                CenterId = 122095,//تجهیزات پزشکی و عینک
                Mobile = "09173838322",
                UserName = "3874099024",
                Password = "Sb@bati@2025",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Workstationid = Workstationid
            });
        }

        /// <summary>
        /// افزودن یه مرکز مثلا ازمایشگاه یا داروخانه و...
        /// </summary>
        public static void FnAddCenter(int CenterId)
        {
            var _InitCenter = FnGetInitilizerCenter(CenterId);
            if (_InitCenter != null)
            {
                SakhadCenter NewCenter = new SakhadCenter(_InitCenter);

                if (FnGetCenter(NewCenter.CenterId) != null)
                {//اگه موجود بود پاکش کن
                    FnRemoveCenter(NewCenter.CenterId);
                }
                else
                {
                    sakhadCenters.Add(NewCenter);
                }
            }
        }

        /// <summary>
        /// پاک کردن یه مرکز
        /// </summary>
        /// <param name="CenterId">شناسه یا کد مرکز</param>
        public static void FnRemoveCenter(int CenterId)
        {
            var FindedCenter = sakhadCenters.Where(x => x.CenterId == CenterId).FirstOrDefault();
            if (FindedCenter != null)
            {
                //اگه بودش پاکش کن از لیست
                sakhadCenters.Remove(FindedCenter);
            }
        }

        /// <summary>
        /// خالی کردن لیست همه مراکز
        /// </summary>
        public static void FnClearAllCenters()
        {
            sakhadCenters.Clear();
        }

        /// <summary>
        /// گرفتن یه مرکز اضافه شده لیست
        /// </summary>
        /// <param name="CenterId"> شناسه یا کد مرکز</param>
        /// <returns></returns>
        public static SakhadCenter FnGetCenter(int CenterId)
        {
            return sakhadCenters.Where(x => x.CenterId == CenterId).FirstOrDefault();
        }

        public static InitilizerCenter FnGetInitilizerCenter(int CenterId)
        {
            return InitCenter.Where(x => x.CenterId == CenterId).FirstOrDefault();
        }
    }
}