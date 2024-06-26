using Contracts.Communicator.Request;
using Contracts.Models;
using Contracts.Policies;

namespace Contracts.Validators
{
    public class ArticleUpdateValidator : ValidatorInterface
    {
        private ArticleUpdateContract article;
        private ErrorDetails error;
        public ArticleUpdateValidator(ArticleUpdateContract article)
        {
            this.article = article;
            this.error = ErrorTypes.Incomplete;
        }

        public ErrorDetails GetError()
        {
            return this.error;
        }

        public bool Validate()
        {
            if (this.article == null)
            {
                this.error = ErrorTypes.Err_BadRequest;
                return false;
            }
            if (this.article.Slug == null || this.article.Slug.Length != ArticlePolicy.SLUG_LENGTH)
            {
                this.error = ErrorTypes.Err_ArticleSlug;
                return false;
            }
            if (this.article.AuthorId <= 0)
            {
                this.error = ErrorTypes.Err_ArticleAuthorId;
                return false;
            }
            if (this.article.Title != null && (this.article.Title.Length == ArticlePolicy.MIN_TITLE_LENGTH || this.article.Title.Length > ArticlePolicy.MAX_TITLE_LENGTH))
            {
                this.error = ErrorTypes.Err_ArticleTitle;
                return false;
            }
            if (this.article.Description != null && (this.article.Description.Length == ArticlePolicy.MIN_DESCRIPTION_LENGTH || this.article.Description.Length > ArticlePolicy.MAX_DESCRIPTION_LENGTH))
            {
                this.error = ErrorTypes.Err_ArticleDescription;
                return false;
            }
            if (this.article.Body != null && (this.article.Body.Length == ArticlePolicy.MIN_BODY_LENGTH || this.article.Body.Length > ArticlePolicy.MAX_BODY_LENGTH))
            {
                this.error = ErrorTypes.Err_ArticleBody;
                return false;
            }
            if (this.article.Tags != null && this.article.Tags.Count > 0)
            {
                // Update article does not allow tags to be updated
                this.error = ErrorTypes.Err_ArticleTags;
                return false;
                /*foreach (string tag in this.article.Tags)
                {
                    if (string.IsNullOrEmpty(tag) || tag.Length < ArticlePolicy.MIN_TAG_LENGTH || tag.Length > ArticlePolicy.MAX_TAG_LENGTH)
                    {
                        this.error = ErrorTypes.Err_ArticleTags;
                        return false;
                    }
                }*/
            }
            this.error = ErrorTypes.Valid;
            return true;
        }
    }
}
