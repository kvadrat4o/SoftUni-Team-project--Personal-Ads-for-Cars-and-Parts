﻿namespace Store.Data
{
    public class ModelConstants
    {
        public const decimal VAT = 20; // 20%

        public const int UserFirstAndLastNameMinLength = 2;
        public const int UserFirstAndLastNameMaxLength = 30;
        public const int UserUserNameMinLength = 3;
        public const int UserUserNameMaxLength = 20;
        public const int UserAvatarPathMaxLength = 350;

        public const int ProductTitleMinLength = 10;
        public const int ProductTitleMaxLength = 130;
        public const int ProductDescriptionMaxLength = 500;
        public const int ProductPicturePathMaxLength = 100;
        public const int ProductPartNumberMinLength = 5;
        public const int ProductPartNumberMaxLength = 15;
        public const int ProductMonthsLive = 1;
        public const int ProductQuantityMinValue = 0;
        public const int ProductQuantityMaxValue = int.MaxValue;

        public const int FeedbackContentMinLength = 10;
        public const int FeedbackContentMaxLength = 100;
        public const int FeedbackRatingMinValue = 1;
        public const int FeedbackRatingMaxValue = 10;

        public const int TownNameMinLength = 3;
        public const int TownNameMaxLength = 30;
        public const int TownPostCodeMaxLength = 10;
        
        public const int AddressStreetMinLength = 3;
        public const int AddressStreetMaxLength = 100;
        
        public const int CarMakeMinLength = 3;
        public const int CarMakeMaxLength = 30;
        public const int CarModelMinLength = 1;
        public const int CarModelMaxLength = 30;

        public const int CommentContentMinLength = 3;
        public const int CommentContentMaxLength = 100;

        public const int CountryNameMinLength = 2;
        public const int CountryNameMaxLength = 30;

        /* Shared Constants */
        public const double MoneyMinValue = 0.0;
        public const double MoneyMaxValue = double.MaxValue;

        /* Administrator account data */
        public const string AdminRoleName = "Admin";
        public const string AdminUsername = "AdminUsername";
        public const string AdminFirstName = "AdminFirstName";
        public const string AdminLastName = "AdminLastName";
        public const string AdminEmail = "admin@mysite.com";
        public const string AdminPassword = "admin123";
    }
}
