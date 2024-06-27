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

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderService> OrderServices { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

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
            entity.HasKey(e => e.ClinicId).HasName("PK__Clinic__A0C8D19BB1917102");

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
            entity.HasKey(e => e.ClinicServiceId).HasName("PK__Clinic_S__916E631C4E1D07C6");

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
                .HasConstraintName("FK__Clinic_Se__clini__4CA06362");

            entity.HasOne(d => d.Service).WithMany(p => p.ClinicServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Clinic_Se__servi__4D94879B");
        });

        modelBuilder.Entity<ClinicUser>(entity =>
        {
            entity.HasKey(e => e.ClinicUserId).HasName("PK__Clinic_U__7EF5AC80C211C106");

            entity.ToTable("Clinic_User");

            entity.Property(e => e.ClinicUserId).HasColumnName("clinic_user_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ClinicUsers)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Clinic_Us__clini__656C112C");

            entity.HasOne(d => d.User).WithMany(p => p.ClinicUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Clinic_Us__user___66603565");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85E51FD8DB");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BackIdCard)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("back_id_card");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FrontIdCard)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("front_id_card");
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
            entity.HasKey(e => e.CustomerClinicId).HasName("PK__Customer__971AD285D91C0557");

            entity.ToTable("Customer_Clinic");

            entity.Property(e => e.CustomerClinicId).HasColumnName("customer_clinic_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Clinic).WithMany(p => p.CustomerClinics)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Customer___clini__3C69FB99");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerClinics)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Customer___custo__3B75D760");
        });

        modelBuilder.Entity<DentistSlot>(entity =>
        {
            entity.HasKey(e => e.DentistSlotId).HasName("PK__Dentist___F7C6C8C3D90D2CAC");

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
                .HasConstraintName("FK__Dentist_S__denti__534D60F1");

            entity.HasOne(d => d.Room).WithMany(p => p.DentistSlots)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Dentist_S__room___5441852A");
        });

        modelBuilder.Entity<Examination>(entity =>
        {
            entity.HasKey(e => e.ExaminationId).HasName("PK__Examinat__BCD82530B9368D72");

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
                .HasConstraintName("FK__Examinati__denti__5812160E");

            entity.HasOne(d => d.DentistSlot).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.DentistSlotId)
                .HasConstraintName("FK__Examinati__denti__59063A47");

            entity.HasOne(d => d.ExaminationProfile).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.ExaminationProfileId)
                .HasConstraintName("FK__Examinati__exami__571DF1D5");
        });

        modelBuilder.Entity<ExaminationProfile>(entity =>
        {
            entity.HasKey(e => e.ExaminationProfileId).HasName("PK__Examinat__2B0A0490E654E411");

            entity.ToTable("Examination_Profile");

            entity.Property(e => e.ExaminationProfileId).HasColumnName("examination_profile_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Customer).WithMany(p => p.ExaminationProfiles)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Examinati__custo__3F466844");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.MedicalRecordId).HasName("PK__Medical___05C4C30A9F4BB550");

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
                .HasConstraintName("FK__Medical_R__exami__693CA210");

            entity.HasOne(d => d.RecordType).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.RecordTypeId)
                .HasConstraintName("FK__Medical_R__recor__6A30C649");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__E7148EBBB4215C76");

            entity.ToTable("Medicine");

            entity.Property(e => e.MedicineId).HasColumnName("medicine_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Effect)
                .HasMaxLength(255)
                .HasColumnName("effect");
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
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("unit");
        });

        modelBuilder.Entity<MedicinePrescription>(entity =>
        {
            entity.HasKey(e => e.MedicinePrescriptionId).HasName("PK__Medicine__9354DD606C725124");

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
                .HasConstraintName("FK__Medicine___medic__6FE99F9F");

            entity.HasOne(d => d.Prescription).WithMany(p => p.MedicinePrescriptions)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK__Medicine___presc__70DDC3D8");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__46596229320DEE03");

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
                .HasConstraintName("FK__Order__examinati__5BE2A6F2");
        });

        modelBuilder.Entity<OrderService>(entity =>
        {
            entity.HasKey(e => e.OrderServiceId).HasName("PK__Order_Se__88196EDD9BB7C919");

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
                .HasConstraintName("FK__Order_Ser__order__619B8048");

            entity.HasOne(d => d.Service).WithMany(p => p.OrderServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Order_Ser__servi__628FA481");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EAEC04A8B1");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentDetail)
                .HasMaxLength(255)
                .HasColumnName("payment_detail");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__order_i__5EBF139D");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__3EE444F85646BCE4");

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
                .HasConstraintName("FK__Prescript__exami__6D0D32F4");
        });

        modelBuilder.Entity<RecordType>(entity =>
        {
            entity.HasKey(e => e.RecordTypeId).HasName("PK__Record_T__3F68ADEC3FA96339");

            entity.ToTable("Record_Type");

            entity.Property(e => e.RecordTypeId).HasColumnName("record_type_id");
            entity.Property(e => e.RecordName)
                .HasMaxLength(255)
                .HasColumnName("record_name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CCFC10CAFA");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__19675A8A78FB37F4");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(50)
                .HasColumnName("room_number");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Room__clinic_id__47DBAE45");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__3E0DB8AF6345B727");

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
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F8C9B4115");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
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
                .HasConstraintName("FK__User__role_id__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
