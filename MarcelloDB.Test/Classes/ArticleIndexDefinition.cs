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

        public UniqueIndexedValue<Article, string> Reference { get; set; }

        public UniqueIndexedValue<Article, string> CustomReference
        {
            get
            {
                return UniqueIndexedValue(a => a.Reference);
            }
        }

        public IndexedValue<Article, string> FullDescription
        {
            get
            {
                return IndexedValue(
                    article => String.Format("{0}-{1}", article.Name, article.Description)
                );
            }
        }

        public IndexedValue<Article, string, string> CodeAndName
        {
            get
            {
                return IndexedValue(
                    article =>CompoundValue(article.Code, article.Name)
                );
            }
        }

        public IndexedValue<Article, int, string, string> IdCodeAndName
        {
            get
            {
                return IndexedValue(
                    article => CompoundValue(article.ID, article.Code, article.Name)
                    );
            }
        }

        public IndexedValue<Article, string> ShortName
        {
            get
            {
                return IndexedValue(
                    article => article.ShortName,
                    article => article.ShortName != null
                );
            }
        }
    }
}

