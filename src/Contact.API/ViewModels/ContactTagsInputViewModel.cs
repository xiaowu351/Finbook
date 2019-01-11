using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.ViewModels
{
    /// <summary>
    /// 标签的ViewModel
    /// </summary>
    public class ContactTagsInputViewModel
    {
        public int contactId { get; set; }

        public List<string> Tags { get; set; }
    }
}
