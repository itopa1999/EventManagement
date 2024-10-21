using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public enum UserType
    {
        Admin,
        Organizer,
        Attendance
    }

    public enum TicketType
    {
        Free,
        Paid
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        BankTransfer,
        PayPal,
        MobilePayment // Other methods as needed
    }

    public enum InvitationStatus
    {
        Pending,
        Sent,
        Accepted,
        Declined
    }

    public enum ReminderType
    {
        EventStart,
        CheckIn,
        FeedbackRequest
    }


    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum TransactionType
    {
        debit,
        credit
    }


    public enum EventType
    {
        Conference,
        Seminar,
        Workshop,
        ProductLaunche,
        TradeShow,
        TeamBuildingActivitie,
        NetworkingEvent,
        ExecutiveRetreat,
        Wedding,
        Anniversarie,
        BirthdayPartie,
        BabyShower,
        FamilyReunion,
        Graduation,
        Festival,
        ArtFestival,
        Parade,
        CharityEvent,
        Fundraiser,
        LocalMarket,
        Class,
        Webinar,
        Lecture,
        TrainingSession,
        Concert,
        TheatricalPerformance,
        ComedyShow,
        MovieScreening,
        SportsEvent,
        Baptism,
        Confirmation,
        ReligiousRetreat,
        CommunityService,
        SeasonalEvent,
        HolidayParties,
        SummerCamp,
        HealthWellnessEvent,
        YogaRetreat,
        FitnessWorkshop,
        HealthFair,
        Meetup,
        OnlineConference,
        VirtualWorkshop,
        LiveStreamingEvent,
    }


}