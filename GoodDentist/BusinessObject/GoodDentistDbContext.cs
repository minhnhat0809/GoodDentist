using System;
using System.Collections.Generic;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject;

public partial class GoodDentistDbContext : DbContext
{
    public GoodDentistDbContext()
    {
    }

    public GoodDentistDbContext(DbContextOptions<GoodDentistDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<ClinicService> ClinicServices { get; set; }

    public virtual DbSet<ClinicUser> ClinicUsers { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerClinic> CustomerClinics { get; set; }

    public virtual DbSet<DentistSlot> DentistSlots { get; set; }

    public virtual DbSet<Examination> Examinations { get; set; }

    public virtual DbSet<ExaminationProfile> ExaminationProfiles { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicinePrescription> MedicinePrescriptions { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderService> OrderServices { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentAll> PaymentAlls { get; set; }

    public virtual DbSet<PaymentPrescription> PaymentPrescriptions { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<RecordType> RecordTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.ClinicId).HasName("PK__Clinic__A0C8D19B0D9AC3C1");

            entity.ToTable("Clinic");

            entity.Property(e => e.ClinicId)
                .ValueGeneratedNever()
                .HasColumnName("clinic_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.ClinicName)
                .HasMaxLength(255)
                .HasColumnName("clinic_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<ClinicService>(entity =>
        {
            entity.HasKey(e => e.ClinicServiceId).HasName("PK__Clinic_S__916E631C8096BEEC");

            entity.ToTable("Clinic_Service");

            entity.Property(e => e.ClinicServiceId).HasColumnName("clinic_service_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ClinicServices)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Clinic_Se__clini__5070F446");

            entity.HasOne(d => d.Service).WithMany(p => p.ClinicServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Clinic_Se__servi__5165187F");
        });

        modelBuilder.Entity<ClinicUser>(entity =>
        {
            entity.HasKey(e => e.ClinicUserId).HasName("PK__Clinic_U__7EF5AC80C4C7854C");

            entity.ToTable("Clinic_User");

            entity.Property(e => e.ClinicUserId).HasColumnName("clinic_user_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ClinicUsers)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Clinic_Us__clini__66603565");

            entity.HasOne(d => d.User).WithMany(p => p.ClinicUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Clinic_Us__user___6754599E");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB853D33A32D");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Anamnesis)
                .HasMaxLength(255)
                .HasColumnName("anamnesis");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<CustomerClinic>(entity =>
        {
            entity.HasKey(e => e.CustomerClinicId).HasName("PK__Customer__971AD285255E3918");

            entity.ToTable("Customer_Clinic");

            entity.Property(e => e.CustomerClinicId).HasColumnName("customer_clinic_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Clinic).WithMany(p => p.CustomerClinics)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Customer___clini__412EB0B6");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerClinics)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Customer___custo__403A8C7D");
        });

        modelBuilder.Entity<DentistSlot>(entity =>
        {
            entity.HasKey(e => e.DentistSlotId).HasName("PK__Dentist___F7C6C8C3674E3850");

            entity.ToTable("Dentist_Slot");

            entity.Property(e => e.DentistSlotId).HasColumnName("dentist_slot_id");
            entity.Property(e => e.DentistId).HasColumnName("dentist_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("time_end");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("time_start");

            entity.HasOne(d => d.Dentist).WithMany(p => p.DentistSlots)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__Dentist_S__denti__5441852A");

            entity.HasOne(d => d.Room).WithMany(p => p.DentistSlots)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Dentist_S__room___5535A963");
        });

        modelBuilder.Entity<Examination>(entity =>
        {
            entity.HasKey(e => e.ExaminationId).HasName("PK__Examinat__BCD82530E8C4EB12");

            entity.ToTable("Examination");

            entity.Property(e => e.ExaminationId).HasColumnName("examination_id");
            entity.Property(e => e.DentistId).HasColumnName("dentist_id");
            entity.Property(e => e.DentistSlotId).HasColumnName("dentist_slot_id");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.ExaminationProfileId).HasColumnName("examination_profile_id");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("time_end");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("time_start");

            entity.HasOne(d => d.Dentist).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__Examinati__denti__59063A47");

            entity.HasOne(d => d.DentistSlot).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.DentistSlotId)
                .HasConstraintName("FK__Examinati__denti__59FA5E80");

            entity.HasOne(d => d.ExaminationProfile).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.ExaminationProfileId)
                .HasConstraintName("FK__Examinati__exami__5812160E");
        });

        modelBuilder.Entity<ExaminationProfile>(entity =>
        {
            entity.HasKey(e => e.ExaminationProfileId).HasName("PK__Examinat__2B0A0490DD707DBC");

            entity.ToTable("Examination_Profile");

            entity.Property(e => e.ExaminationProfileId).HasColumnName("examination_profile_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DentistId).HasColumnName("dentist_id");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Customer).WithMany(p => p.ExaminationProfiles)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Examinati__custo__440B1D61");

            entity.HasOne(d => d.Dentist).WithMany(p => p.ExaminationProfiles)
                .HasForeignKey(d => d.DentistId)
                .HasConstraintName("FK__Examinati__denti__44FF419A");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.MedicalRecordId).HasName("PK__Medical___05C4C30A36DE65DD");

            entity.ToTable("Medical_Record");

            entity.Property(e => e.MedicalRecordId).HasColumnName("medical_record_id");
            entity.Property(e => e.ExaminationId).HasColumnName("examination_id");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasColumnName("notes");
            entity.Property(e => e.RecordTypeId).HasColumnName("record_type_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url");

            entity.HasOne(d => d.Examination).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.ExaminationId)
                .HasConstraintName("FK__Medical_R__exami__6A30C649");

            entity.HasOne(d => d.RecordType).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.RecordTypeId)
                .HasConstraintName("FK__Medical_R__recor__6B24EA82");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__E7148EBB62C3C488");

            entity.ToTable("Medicine");

            entity.Property(e => e.MedicineId).HasColumnName("medicine_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.MedicineName)
                .HasMaxLength(255)
                .HasColumnName("medicine_name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Unit)
                .HasMaxLength(10)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<MedicinePrescription>(entity =>
        {
            entity.HasKey(e => e.MedicinePrescriptionId).HasName("PK__Medicine__9354DD607893E77F");

            entity.ToTable("Medicine_Prescription");

            entity.Property(e => e.MedicinePrescriptionId).HasColumnName("medicine_prescription_id");
            entity.Property(e => e.MedicineId).HasColumnName("medicine_id");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicinePrescriptions)
                .HasForeignKey(d => d.MedicineId)
                .HasConstraintName("FK__Medicine___medic__70DDC3D8");

            entity.HasOne(d => d.Prescription).WithMany(p => p.MedicinePrescriptions)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK__Medicine___presc__71D1E811");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842F51AD5E27");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsPublic).HasColumnName("is_public");
            entity.Property(e => e.NotificationMessage)
                .HasMaxLength(255)
                .HasColumnName("notification_message");
            entity.Property(e => e.NotificationTitle)
                .HasMaxLength(255)
                .HasColumnName("notification_title");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__4659622939FDB05B");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("date_time");
            entity.Property(e => e.ExaminationId).HasColumnName("examination_id");
            entity.Property(e => e.OrderName)
                .HasMaxLength(255)
                .HasColumnName("order_name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Examination).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ExaminationId)
                .HasConstraintName("FK__Order__examinati__5CD6CB2B");
        });

        modelBuilder.Entity<OrderService>(entity =>
        {
            entity.HasKey(e => e.OrderServiceId).HasName("PK__Order_Se__88196EDD082459FF");

            entity.ToTable("Order_Service");

            entity.Property(e => e.OrderServiceId).HasColumnName("order_service_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderServices)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Order_Ser__order__5FB337D6");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Order_Ser__servi__60A75C0F");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EADC55C462");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.OrderServiceId).HasColumnName("order_service_id");
            entity.Property(e => e.PaymentDetail)
                .HasMaxLength(255)
                .HasColumnName("payment_detail");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.OrderService).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderServiceId)
                .HasConstraintName("FK__Payment__order_s__6383C8BA");
        });

        modelBuilder.Entity<PaymentAll>(entity =>
        {
            entity.HasKey(e => e.PaymentAllId).HasName("PK__Payment___8074B6D34E6DA1EE");

            entity.ToTable("Payment_All");

            entity.Property(e => e.PaymentAllId).HasColumnName("payment_all_id");
            entity.Property(e => e.PaymentDetail)
                .HasMaxLength(255)
                .HasColumnName("payment_detail");
            entity.Property(e => e.PaymentId).HasColumnName("paymentId");
            entity.Property(e => e.PaymentPrescriptionId).HasColumnName("payment_prescription_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentAlls)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Payment_A__payme__778AC167");

            entity.HasOne(d => d.PaymentPrescription).WithMany(p => p.PaymentAlls)
                .HasForeignKey(d => d.PaymentPrescriptionId)
                .HasConstraintName("FK__Payment_A__payme__787EE5A0");
        });

        modelBuilder.Entity<PaymentPrescription>(entity =>
        {
            entity.HasKey(e => e.PaymentPrescriptionId).HasName("PK__Payment___6D84E41455BB623B");

            entity.ToTable("Payment_Prescription");

            entity.Property(e => e.PaymentPrescriptionId).HasColumnName("payment_prescription_id");
            entity.Property(e => e.PaymentDetail)
                .HasMaxLength(255)
                .HasColumnName("payment_detail");
            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PaymentPrescriptions)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK__Payment_P__presc__74AE54BC");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__3EE444F800BEB99A");

            entity.ToTable("Prescription");

            entity.Property(e => e.PrescriptionId).HasColumnName("prescription_id");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("date_time");
            entity.Property(e => e.ExaminationId).HasColumnName("examination_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.Examination).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.ExaminationId)
                .HasConstraintName("FK__Prescript__exami__6E01572D");
        });

        modelBuilder.Entity<RecordType>(entity =>
        {
            entity.HasKey(e => e.RecordTypeId).HasName("PK__Record_T__3F68ADEC5ACAEDED");

            entity.ToTable("Record_Type");

            entity.Property(e => e.RecordTypeId).HasColumnName("record_type_id");
            entity.Property(e => e.RecordName)
                .HasMaxLength(255)
                .HasColumnName("record_name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC86CAE4F8");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__19675A8A90EB1EF5");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(50)
                .HasColumnName("room_number");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Room__clinic_id__4BAC3F29");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__3E0DB8AF3F68FACC");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("service_name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F1711B9EA");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasColumnName("avatar");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__role_id__3B75D760");

            entity.HasMany(d => d.Notifications).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserNotification",
                    r => r.HasOne<Notification>().WithMany()
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__User_Noti__notif__7E37BEF6"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__User_Noti__user___7D439ABD"),
                    j =>
                    {
                        j.HasKey("UserId", "NotificationId").HasName("PK__User_Not__57BBAF4D4B3D3888");
                        j.ToTable("User_Notification");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("NotificationId").HasColumnName("notification_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
