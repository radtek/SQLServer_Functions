using Excel.Lib;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;

/// <summary>
/// CLR User-Defined Functions in C# inside SQL Server
/// Same fashion than
/// http://westclintech.com/SQL-Server-Financial-Functions/SQL-Server-XIRR-function
/// 
/// https://docs.microsoft.com/en-us/sql/relational-databases/clr-integration-database-objects-user-defined-functions/clr-user-defined-functions?view=sql-server-2017
/// </summary>
public partial class UserDefinedFunctions
{

    public const double VAT_TAX = .020;
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble addVAT(SqlDouble originalAmount)
    {
        SqlDouble taxAmount = originalAmount * VAT_TAX;

        return originalAmount + taxAmount;
        //        return new SqlString (string.Empty);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble remVAT(SqlDouble amount)
    {
        SqlDouble originalAmount = amount / (1 + VAT_TAX);

        return originalAmount;
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble getVAT()
    {
        return VAT_TAX;
    }


    [SqlFunction(DataAccess = DataAccessKind.Read,
                FillRowMethodName = "Invs_FillRow",
                TableDefinition = "InvsIT nvarchar(5), InvsIN nvarchar(20), InvsKey nvarchar(100),InvsId nvarchar(4000),InvsReference1 nvarchar(200), InvsReference2 nvarchar(200), Phase nvarchar(10)")]
    public static IEnumerable ActiveContract()
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select investment_support.internal_type AS InvsIT,investment_support.internal_number AS InvsIN, " +
                      "investment_support.internal_type + investment_support.internal_number AS InvsKey," +
                      "ISNULL(investment_support.label, N'') AS InvsId, ISNULL(investment_support.insurer_reference, N'') AS InvsReference1," +
                      "ISNULL(investment_support.ref2, N'') AS InvsReference2," +
                      "'En cours' as Phase FROM investment_support where(current_phase = 'En cours' and manager <> 'ARCHIVE' )", conn);


            ArrayList resultCollection = new ArrayList();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    resultCollection.Add(new List<Object> {
                            reader.GetSqlString(0),
                            reader.GetSqlString(1),
                            reader.GetSqlString(2),
                            reader.GetSqlString(3),
                            reader.GetSqlString(4),
                            reader.GetSqlString(5),
                            reader.GetSqlString(6)
                             } ) ;
                }
            }


            return resultCollection;
        }
    }
        public static void Invs_FillRow(
           object resultObj,
           out SqlString InvsIT, out SqlString InvsIN, out SqlString InvsKey, out SqlString InvsId, out SqlString InvsReference1, out SqlString InvsReference2, out SqlString Phase)
        {
        List<Object> result = (List<Object>)resultObj;

        InvsIT = (SqlString)result[0];
        InvsIN = (SqlString)result[1];
        InvsKey = (SqlString)result[2];
        InvsId = (SqlString)result[3];
        InvsReference1 = (SqlString)result[4];
        InvsReference2 = (SqlString)result[5];
        Phase = (SqlString)result[6];
    }

    


    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble XIRR(List<SqlDouble> cashflows, List<SqlDateTime> dates, SqlDouble estimate)
    {
        List<Double> c1 = cashflows.Select(x => x.Value).ToList();
        List<DateTime> c2 = dates.Select(x => x.Value).ToList();

        if (estimate == SqlDouble.Null)
        {
            return new XIRR(c1, c2).Calculate();
        }
        else
        {
            return new XIRR(c1, c2, estimate.Value).Calculate();
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDouble XIRR(List<SqlDouble> cashflows, List<SqlDateTime> dates, SqlInt16 maxFloatingPoints, SqlInt16 maxNbDecimalDigits)
    {
        double precision; int decimals;

        if (maxFloatingPoints == SqlInt16.Null)
        {
            precision = 0.00001;
        }
        else
        {
            precision = Math.Pow(10, -maxFloatingPoints.Value);
        }

        if (maxNbDecimalDigits == SqlDouble.Null)
        {
            decimals = 10;
        }
        else
        {
            decimals = maxNbDecimalDigits.Value;
        }

        List<Double> c1 = cashflows.Select(x => x.Value).ToList();
        List<DateTime> c2 = dates.Select(x => x.Value).ToList();

        return new XIRR(c1, c2).Calculate(precision, decimals);

    }


}
