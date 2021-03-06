﻿
namespace C4F.VistaP2P.Common
{
    public enum SystemState
    {
        LoggedOut,
        LoggingIn,
        LoggedIn,
    }

    //the possible states the streamed class can be in.
    public enum StreamedStateType
    {
        Initial,
        Communicating,
    }


    //I recovered these by calling folder.GetDetailsOf(null, numOfInterest);
    public enum FileInfo
    {
        Name = 0,
        Size = 1,
        Type = 2,
        Date_modified = 3,
        Date_created = 4,
        Date_accessed = 5,
        Attributes = 6,
        Offline_status = 7,
        Offline_availability = 8,
        Perceived_type = 9,
        Owner = 10,
        Kinds = 11,
        Date_taken = 12,
        Artists = 13,
        Album = 14,
        Year = 15,
        Genre = 16,
        Conductors = 17,
        Tags = 18,
        Rating = 19,
        Authors = 20,
        Title = 21,
        Subject = 22,
        Categories = 23,
        Comments = 24,
        Copyright = 25,
        //  # = 26,
        Length = 27,
        Bit_rate = 28,
        Protected = 29,
        Camera_model = 30,
        Dimensions = 31,
        Camera_maker = 32,
        Company = 33,
        File_description = 34,
        Program_name = 35,
        Duration = 36,
        Is_online = 37,
        Is_recurring = 38,
        Location = 39,
        Optional_attendee_addresses = 40,
        Optional_attendees = 41,
        Organizer_address = 42,
        Organizer_name = 43,
        Reminder_time = 44,
        Required_attendee_addresses = 45,
        Required_attendees = 46,
        Resources = 47,
        Free_busy_status = 48,
        Total_size = 49,
        Account_name = 50,
        Computer = 51,
        Anniversary = 52,
        Assistants_name = 53,
        Assistants_phone = 54,
        Birthday = 55,
        Business_address = 56,
        Business_city = 57,
        Business_country_region = 58,
        Business_P_O__box = 59,
        Business_postal_code = 60,
        Business_state_or_province = 61,
        Business_street = 62,
        Business_fax = 63,
        Business_home_page = 64,
        Business_phone = 65,
        Callback_number = 66,
        Car_phone = 67,
        Children = 68,
        Company_main_phone = 69,
        Department = 70,
        E_mail_Address = 71,
        E_mail2 = 72,
        E_mail3 = 73,
        E_mail_list = 74,
        E_mail_display_name = 75,
        File_as = 76,
        First_name = 77,
        Full_name = 78,
        Gender = 79,
        Given_name = 80,
        Hobbies = 81,
        Home_address = 82,
        Home_city = 83,
        Home_country_region = 84,
        Home_P_O__box = 85,
        Home_postal_code = 86,
        Home_state_or_province = 87,
        Home_street = 88,
        Home_fax = 89,
        Home_phone = 90,
        IM_addresses = 91,
        Initials = 92,
        Job_title = 93,
        Label = 94,
        Last_name = 95,
        Mailing_address = 96,
        Middle_name = 97,
        Cell_phone = 98,
        Nickname = 99,
        Office_location = 100,
        Other_address = 101,
        Other_city = 102,
        Other_country_region = 103,
        Other_PO_box = 104,
        Other_postal_code = 105,
        Other_state_or_province = 106,
        Other_street = 107,
        Pager = 108,
        Personal_title = 109,
        City = 110,
        Country_region = 111,
        PO_box = 112,
        Postal_code = 113,
        State_or_province = 114,
        Street = 115,
        Primary_e_mail = 116,
        Primary_phone = 117,
        Profession = 118,
        Spouse = 119,
        Suffix = 120,
        TTY_TTD_phone = 121,
        Telex = 122,
        Webpage = 123,
        Status = 124,
        Content_type = 125,
        Date_acquired = 126,
        Date_archived = 127,
        Date_completed = 128,
        Date_imported = 129,
        Client_ID = 130,
        Contributors = 131,
        Content_created = 132,
        Last_printed = 133,
        Date_last_saved = 134,
        Division = 135,
        Document_ID = 136,
        Pages = 137,
        Slides = 138,
        Total_editing_time = 139,
        Word_count = 140,
        Due_date = 141,
        End_date = 142,
        File_count = 143,
        Filename = 144,
        File_version = 145,
        Flag_color = 146,
        Flag_status = 147,
        Space_free = 148,
        Bit_depth = 149,
        Horizontal_resolution = 150,
        Width = 151,
        Vertical_resolution = 152,
        Height = 153,
        Importance = 154,
        Is_attachment = 155,
        Is_deleted = 156,
        Has_flag = 157,
        Is_completed = 158,
        Incomplete = 159,
        Read_status = 160,
        Shared = 161,
        Creator = 162,
        Date = 163,
        Folder_name = 164,
        Folder_path = 165,
        Folder = 166,
        Participants = 167,
        Path = 168,
        Contact_names = 169,
        Entry_type = 170,
        Language = 171,
        Date_visited = 172,
        Description = 173,
        Link_status = 174,
        Link_target = 175,
        URL = 176,
        Media_created = 177,
        Date_released = 178,
        Encoded_by = 179,
        Producers = 180,
        Publisher = 181,
        Subtitle = 182,
        User_web_URL = 183,
        Writers = 184,
        Attachments = 185,
        Bcc_addresses = 186,
        Bcc_names = 187,
        Cc_addresses = 188,
        Cc_names = 189,
        Conversation_ID = 190,
        Date_received = 191,
        Date_sent = 192,
        From_addresses = 193,
        From_names = 194,
        Has_attachments = 195,
        Sender_address = 196,
        Sender_name = 197,
        Store = 198,
        To_addresses = 199,
        To_do_title = 200,
        To_names = 201,
        Mileage = 202,
        Album_artist = 203,
        Beats_per_minute = 204,
        Composers = 205,
        Initial_key = 206,
        Mood = 207,
        Part_of_set = 208,
        Period = 209,
        Color = 210,
        Parental_rating = 211,
        Parental_rating_reason = 212,
        Space_used = 213,
        EXIF_version = 214,
        Event = 215,
        Exposure_bias = 216,
        Exposure_program = 217,
        Exposure_time = 218,
        F_stop = 219,
        Flash_mode = 220,
        Focal_length = 221,
        x35mm_focal_length = 222,
        ISO_speed = 223,
        Lens_maker = 224,
        Lens_model = 225,
        Light_source = 226,
        Max_aperture = 227,
        Metering_mode = 228,
        Orientation = 229,
        Program_mode = 230,
        Saturation = 231,
        Subject_distance = 232,
        White_balance = 233,
        Priority = 234,
        Project = 235,
        Channel_number = 236,
        Episode_name = 237,
        Closed_captioning = 238,
        Rerun = 239,
        SAP = 240,
        Broadcast_date = 241,
        Program_description = 242,
        Recording_time = 243,
        Station_call_sign = 244,
        Station_name = 245,
        Auto_summary = 246,
        Summary = 247,
        Search_ranking = 248,
        Sensitivity = 249,
        Shared_with = 250,
        Product_name = 251,
        Product_version = 252,
        Source = 253,
        Start_date = 254,
        Billing_information = 255,
        Complete = 256,
        Task_owner = 257,
        Total_file_size = 258,
        Legal_trademarks = 259,
        Video_compression = 260,
        Directors = 261,
        Data_rate = 262,
        Frame_height = 263,
        Frame_rate = 264,
        Frame_width = 265,
        Total_bitrate = 266,
    }
}
