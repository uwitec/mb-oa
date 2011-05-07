namespace EfSearchModel
{
    using System;
    using System.Collections.Generic;
    using Model;

    public class LessDateTransformProvider : ITransformProvider
    {
        public bool Match(SearchItem item, Type type)
        {
            return true;
            
        }

        public IEnumerable<SearchItem> Transform(SearchItem item, Type type)
        {
            throw new NotImplementedException();
        }
    }
}