USE [T3Andon]
GO

/****** Object:  Trigger [dbo].[TRG_UretimPlanliDurus_Insert]    Script Date: 3.10.2022 15:36:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 
CREATE TRIGGER [dbo].[TRG_UretimPlanliDurus_Insert] ON [dbo].[T3_UretimPlanliDurus] 
AFTER INSERT 
AS 
Begin

DECLARE @Id uniqueidentifier, @UretimId uniqueidentifier, @CalismaId uniqueidentifier, @Kod nvarchar(50), @Baslangic datetime2(7), @Bitis datetime2(7), @Barkod nvarchar(50), @Miktar int, @DkGercek int, @DkHedef int, @DkHedefG int
DECLARE @Uretim_Baslangic datetime2(7)
  
 SELECT	@Id = x.Id , @UretimId = x.UretimId, @Kod = x.Kod, @Baslangic = x.Baslangic, @Bitis = x.Bitis FROM Inserted AS x 

 SELECT @Uretim_Baslangic = Baslangic FROM T3_Uretim WHERE Id = @UretimId
 
 UPDATE T3_UretimPlanliDurus SET Baslangic = @Uretim_Baslangic, Zaman = DATEDIFF(SECOND, @Uretim_Baslangic, Bitis) WHERE Id = @Id


End
GO

ALTER TABLE [dbo].[T3_UretimPlanliDurus] ENABLE TRIGGER [TRG_UretimPlanliDurus_Insert]
GO


