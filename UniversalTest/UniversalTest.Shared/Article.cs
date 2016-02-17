using MarcelloDB.Collections;
using MarcelloDB.Index;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalTest
{
    public class ArticleIndex : IndexDefinition<Article>
    {
        public IndexedValue<Article, string> Name { get; set; }
    }

    public class Article
    {
        public int ID { get; set; }
        public string Name { get; set; }  
    }
}
