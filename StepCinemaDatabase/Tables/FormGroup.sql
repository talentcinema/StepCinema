CREATE TABLE [dbo].[FormGroup] (
    [FormGroupId]   VARCHAR (50)  NOT NULL,
    [FormGroupName] VARCHAR (200) NOT NULL,
    [Order]         INT           NULL,
    [Active]        BIT           NOT NULL DEFAULT 1,
    CONSTRAINT [PK_FormGroup] PRIMARY KEY CLUSTERED ([FormGroupId] ASC)
);
