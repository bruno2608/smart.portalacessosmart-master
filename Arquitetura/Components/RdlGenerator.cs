using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Arquitetura.Components
{

    public class RdlGenerator
    {
        private List<string> m_allFields;
        private List<KeyValuePair<string, bool>> m_selectedFields;
        private List<KeyValuePair<string, bool>> m_headers;

        public List<string> AllFields
        {
            get { return m_allFields; }
            set { m_allFields = value; }
        }

        public List<KeyValuePair<string, bool>> SelectedFields
        {
            get { return m_selectedFields; }
            set { m_selectedFields = value; }
        }

        public List<KeyValuePair<string, bool>> Headers
        {
            get { return m_headers; }
            set { m_headers = value; }
        }
        private Report CreateReport()
        {
            Report report = new Report();
            report.Items = new object[]
                {
                    CreateDataSources(),
                    CreateBody(),
                    CreateDataSets(),
                    "6.5in",
                };
            report.ItemsElementName = new ItemsChoiceType37[]
                {
                    ItemsChoiceType37.DataSources,
                    ItemsChoiceType37.Body,
                    ItemsChoiceType37.DataSets,
                    ItemsChoiceType37.Width,
                };
            return report;
        }

        private DataSourcesType CreateDataSources()
        {
            DataSourcesType dataSources = new DataSourcesType();
            dataSources.DataSource = new DataSourceType[] { CreateDataSource() };
            return dataSources;
        }

        private DataSourceType CreateDataSource()
        {
            DataSourceType dataSource = new DataSourceType();
            dataSource.Name = "DummyDataSource";
            dataSource.Items = new object[] { CreateConnectionProperties() };
            return dataSource;
        }

        private ConnectionPropertiesType CreateConnectionProperties()
        {
            ConnectionPropertiesType connectionProperties = new ConnectionPropertiesType();
            connectionProperties.Items = new object[]
                {
                    "",
                    "SQL",
                };
            connectionProperties.ItemsElementName = new ItemsChoiceType[]
                {
                    ItemsChoiceType.ConnectString,
                    ItemsChoiceType.DataProvider,
                };
            return connectionProperties;
        }

        private BodyType CreateBody()
        {
            BodyType body = new BodyType();
            body.Items = new object[]
                {
                    CreateReportItems(),
                    "1in",
                };
            body.ItemsElementName = new ItemsChoiceType30[]
                {
                    ItemsChoiceType30.ReportItems,
                    ItemsChoiceType30.Height,
                };
            return body;
        }

        private ReportItemsType CreateReportItems()
        {
            ReportItemsType reportItems = new ReportItemsType();
            TableRdlGenerator tableGen = new TableRdlGenerator();
            tableGen.Fields = m_selectedFields;
            tableGen.Headers = m_headers;
            reportItems.Items = new object[] { tableGen.CreateTable() };
            return reportItems;
        }

        private DataSetsType CreateDataSets()
        {
            DataSetsType dataSets = new DataSetsType();
            dataSets.DataSet = new DataSetType[] { CreateDataSet() };
            return dataSets;
        }

        private DataSetType CreateDataSet()
        {
            DataSetType dataSet = new DataSetType();
            dataSet.Name = "MyData";
            dataSet.Items = new object[] { CreateQuery(), CreateFields() };
            return dataSet;
        }

        private QueryType CreateQuery()
        {
            QueryType query = new QueryType();
            query.Items = new object[]
                {
                    "DummyDataSource",
                    "",
                };
            query.ItemsElementName = new ItemsChoiceType2[]
                {
                    ItemsChoiceType2.DataSourceName,
                    ItemsChoiceType2.CommandText,
                };
            return query;
        }

        private FieldsType CreateFields()
        {
            FieldsType fields = new FieldsType();

            fields.Field = new FieldType[m_allFields.Count];
            for (int i = 0; i < m_allFields.Count; i++)
            {
                fields.Field[i] = CreateField(m_allFields[i]);
            }

            return fields;
        }

        private FieldType CreateField(String fieldName)
        {
            FieldType field = new FieldType();
            field.Name = fieldName;
            field.Items = new object[] { fieldName };
            field.ItemsElementName = new ItemsChoiceType1[] { ItemsChoiceType1.DataField };
            return field;
        }

        public void WriteXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Report));
            serializer.Serialize(stream, CreateReport());
        }
    }
}
