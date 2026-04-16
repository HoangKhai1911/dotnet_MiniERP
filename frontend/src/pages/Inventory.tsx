import React, { useEffect, useState } from 'react';
import { Table, Card, Statistic, Row, Col, Progress } from 'antd';
import api from '../api';

const Inventory: React.FC = () => {
  const [inventory, setInventory] = useState([]);

  useEffect(() => {
    api.get('/Inventory').then(res => setInventory(res.data));
  }, []);

  const columns = [
    { title: 'Sản phẩm', dataIndex: ['product', 'name'], key: 'prodName' },
    { title: 'Kho', dataIndex: ['warehouse', 'name'], key: 'whName' },
    { 
      title: 'Số lượng tồn', 
      dataIndex: 'quantity', 
      key: 'quantity',
      render: (val: number) => (
        <span style={{ fontWeight: 'bold', color: val < 10 ? 'red' : 'green' }}>{val}</span>
      )
    },
    {
      title: 'Trạng thái',
      render: (_: any, record: any) => (
        <Progress percent={record.quantity > 100 ? 100 : record.quantity} size="small" status={record.quantity < 10 ? 'exception' : 'active'} />
      )
    }
  ];

  return (
    <Card title="Báo cáo Tồn kho thực tế">
      <Table columns={columns} dataSource={inventory} rowKey="id" />
    </Card>
  );
};

export default Inventory;