for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P S1k3-a332@3 -d master -i schema.sql
    if [ $? -eq 0 ]
    then
        echo "setup.sql completed" && pkill sqlservr # kill the /opt/mssql/bin/sqlservr process
        # add delay to ensure process is killed
        sleep 10
        break
    else
        echo "not ready yet..."
        sleep 1
    fi
done

# restart SQL Server in the foreground
/opt/mssql/bin/sqlservr
