using System;
using MarcelloDB.Index;
using MarcelloDB.Collections;

namespace MarcelloDB.Test.Classes
{
    class ArticleIndexes : IndexDefinition<Article>
    {
        public IndexedValue<Article, string> Name { get; set;}

        public IndexedValue<Article, string> Description { get; set; }

        public IndexedValue<Article, string> Category { get; set; }

        public IndexedValue<Article, string> FullDescription {
            get {
                return IndexedValue((Article article) =>
                    {
                        return String.Format("{0}-{1}", article.Name, article.Description);
                    });
            }
        }

        public CompoundIndexedValue<Article, string, string> CodeAndName
        {
            get{
                return CompoundIndexedValue((article) =>
                    {
                        return CompoundValue.Build(article.Code, article.Name);
                    });
            }
        }

        public CompoundIndexedValue<Article, int, string, string> IdCodeAndName
        {
            get{
                return CompoundIndexedValue((article) =>
                    {
                        return CompoundValue.Build(article.ID, article.Code, article.Name);
                    });
            }
        }
    }
}

