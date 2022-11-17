SELECT A.SearchID, COUNT(A.SearchID), MIN(A.WantedID) FROM XXDB (NOLOCK) AS E
WHERE Condition
GROUP BY A.SearchID
HAVING COUNT(A.SearchID)>1
ORDER BY A.SearchID


sqlSendMail üzerinden gönderilmiş ve henuz gonderilmemiş kuyruktaki mailleri görmek için sorgudur
--------------------------------------------------------------------------|
SELECT mailitem_id, recipients, send_request_date, sent_status, sent_date
FROM msdb.dbo.sysmail_allitems nolock
WHERE (sent_status = 'unsent' OR sent_status = 'retrying') 

SELECT mailitem_id, recipients, send_request_date, sent_status, sent_date
FROM msdb.dbo.sysmail_allitems
WHERE sent_status = 'sent' and sent_date > '2022-06-14'

AÇIK KALAN SORGULARI BULMA
--------------------------------------------------------------------------|
SELECT 
P.spid,
right(convert(varchar, dateadd(ms, datediff(ms, P.last_batch, getdate()), '1900-01-01'), 121), 12) as 'batch_duration',
P.program_name,
P.hostname,
P.loginame
FROM master.dbo.sysprocesses (NOLOCK) AS P
WHERE P.spid > 50
AND P.status not in ('background', 'sleeping')
AND P.cmd not in ('AWAITING COMMAND','MIRROR HANDLER','LAZY WRITER','CHECKPOINT SLEEP','RA MANAGER')
order by batch_duration desc

index arama
--------------------------------------------------------------------------|
SELECT * FROM sys.indexes I
JOIN sys.tables T ON I.object_id=T.object_id
JOIN sys.schemas S ON S.schema_id=T.schema_id
WHERE  T.Name='TABLO_ADI' -- Tablo Adı
AND S.Name='dbo' -- Şema Adı

SP LERİ JOBLARDA ARAMA
--------------------------------------------------------------------------|
SELECT j.name
FROM msdb.dbo.sysjobs AS j
WHERE EXISTS
(
SELECT 1 FROM msdb.dbo.sysjobsteps AS s
WHERE s.job_id = j.job_id
AND s.command LIKE '%%'
);
