using Domain.Common;
using DomainShared.Errore;


namespace Domain.NovinEntity.Materials
{
    public class PunProduct : IEntities
    {
        #region Navigation
        public virtual Pun Production { get; set; }
        public virtual Pun RawMaterial { get; set; }
        public Guid ProductionId { get; set; }
        public Guid RawMaterialId { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// درصد ضایعات
        /// </summary>
        public byte WastePercentage { get; set; }
        /// <summary>
        /// درصد استفاده
        /// </summary>
        public byte UsagePercentage { get; set; }

        /// <summary>
        /// نسبت کالای تولیدی به مواد اول
        /// مثال :> تولید هر متر لوله برابر است با پنج کیلو گرانول و صد گرم رنگدانه و یک متر نوار چاپ
        /// </summary>
        public double Ratio { get; set; }
        #endregion

        #region Ctor
        public PunProduct()
        {

        }
        public PunProduct(
            byte wasetPrc,
            byte usagePrc,
            double ratio , 
            Pun rawMat)
        {
            SetRawMaterial(rawMat);
            SetRatio(ratio);
            SetWaste(wasetPrc);
            SetUsage(usagePrc);
        }
        #endregion

        #region Methods
        public PunProduct SetWaste(byte wastePercentage)
        {
            if (wastePercentage > 100)
                throw new ArgumentException(NeErrorCodes.IsLessNumber("درصد ضایعات", "صد"));

            WastePercentage = wastePercentage;
            return this;
        }

        public PunProduct SetUsage(byte usagePercentage)
        {
            if (usagePercentage > 100)
                throw new ArgumentException(NeErrorCodes.IsLessNumber("درصد مصرف", "صد"));

            UsagePercentage = usagePercentage;
            return this;
        }

        public PunProduct SetRatio(double ratio)
        {
            if (ratio > double.MaxValue)
                throw new ArgumentException(NeErrorCodes.IsLessNumber("نسبت", "2000000000"));

            if (ratio <= 0)
                throw new ArgumentException(NeErrorCodes.IsMoreNumber("نسبت", "صفر"));

            Ratio = ratio;
            return this;
        }

        public PunProduct SetRawMaterial(Pun mat)
        {
            RawMaterial = mat;
            return this;
        }
        #endregion
    }
}
