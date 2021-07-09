using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{

    /// <summary>
    /// Tüm entitylerimiz BaseEntity'den türemesi gerekiyor. Aksi  taktirde Repository Patternde kullanamayız. 
    /// </summary>
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public string ExternalId { get; set; }

    }
}