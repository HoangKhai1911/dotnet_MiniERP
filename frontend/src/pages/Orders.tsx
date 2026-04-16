import React, { useEffect, useState } from 'react';
import { Table, Tag, Button, Space, message, Card, Modal } from 'antd'; // Thêm Modal ở đây
import { EyeOutlined, PlusOutlined } from '@ant-design/icons';
import api from '../api';
import SalesOrderForm from '../components/SalesOrderForm'; // Đảm bảo import đúng đường dẫn Form

const Orders: React.FC = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);

  // --- 1. THÊM STATE ĐỂ QUẢN LÝ ĐÓNG/MỞ MODAL ---
  const [isModalOpen, setIsModalOpen] = useState(false);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      // Đảm bảo đường dẫn khớp với config trong api.ts (có /api/v1 chưa)
      const res = await api.get('v1/SalesOrders'); 
      setOrders(res.data);
    } catch (error) {
      message.error('Không thể tải danh sách đơn hàng');
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchOrders();
  }, []);

  const columns = [
    { 
      title: 'Mã ĐH', 
      dataIndex: 'id', 
      key: 'id', 
      render: (id: number) => <strong>#{id}</strong> 
    },
    { 
      title: 'Khách hàng', 
      dataIndex: ['customer', 'name'], 
      key: 'customer', 
      render: (text: string) => text || 'Khách vãng lai' 
    },
    { 
      title: 'Tổng tiền', 
      dataIndex: 'total', 
      key: 'total', 
      render: (val: number) => <span style={{ color: '#f5222d', fontWeight: 500 }}>{val?.toLocaleString()} ₫</span> 
    },
    { 
      title: 'Trạng thái', 
      dataIndex: 'status', 
      key: 'status',
      render: (status: string) => {
        let color = 'default';
        if (status === 'Completed') color = 'green';
        if (status === 'Pending') color = 'orange';
        if (status === 'Shipped') color = 'blue';
        return <Tag color={color}>{status?.toUpperCase()}</Tag>;
      }
    },
    {
      title: 'Thao tác', 
      key: 'action',
      render: (_: any, record: any) => (
        <Space>
          <Button 
            type="primary" 
            ghost 
            icon={<EyeOutlined />}
            onClick={() => console.log("Xem chi tiết đơn hàng ID:", record.id)}
          >
            Chi tiết
          </Button>
        </Space>
      )
    }
  ];

  return (
    <Card 
      title={<h2>Quản lý Đơn hàng (Sales Orders)</h2>} 
      extra={
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          // --- 2. GẮN ONCLICK ĐỂ MỞ MODAL ---
          onClick={() => setIsModalOpen(true)}
        >
          Tạo Đơn hàng mới
        </Button>
      }
      style={{ borderRadius: 8, boxShadow: '0 2px 8px rgba(0,0,0,0.05)' }}
    >
      <Table 
        columns={columns} 
        dataSource={orders} 
        rowKey="id" 
        loading={loading} 
        pagination={{ pageSize: 10 }}
      />

      {/* --- 3. THÊM COMPONENT MODAL CHỨA FORM --- */}
      <Modal
        title="Tạo mới đơn hàng"
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        footer={null} // Ẩn nút mặc định của Modal vì trong Form đã có nút Lưu rồi
        width={800}
        destroyOnClose // Tự động xóa dữ liệu cũ trong Form khi đóng/mở lại
      >
        <SalesOrderForm 
          onSuccess={() => {
            setIsModalOpen(false); // Đóng modal khi lưu thành công
            fetchOrders(); // Tải lại danh sách đơn hàng mới
          }} 
        />
      </Modal>
    </Card>
  );
};

export default Orders;