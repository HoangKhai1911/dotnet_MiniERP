import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, message, Space, Typography } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import axios from 'axios';

const { Title } = Typography;

const Customers: React.FC = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form] = Form.useForm();

  // Gọi API lấy danh sách
  const fetchCustomers = async () => {
    setLoading(true);
    try {
      const response = await axios.get('http://localhost:5000/api/Customers');
      setCustomers(response.data);
    } catch (error) {
      message.error('Không thể tải danh sách khách hàng!');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCustomers();
  }, []);

  // Xử lý submit form thêm mới
  const handleFinish = async (values: any) => {
    try {
      // Gọi đúng API bạn vừa test trên Swagger
      await axios.post('http://localhost:5000/api/Customers', values);
      message.success('Thêm khách hàng thành công!');
      setIsModalOpen(false);
      form.resetFields();
      fetchCustomers(); // Tải lại bảng để cập nhật dữ liệu mới
    } catch (error) {
      message.error('Có lỗi xảy ra khi thêm khách hàng.');
    }
  };

  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id', width: 80, align: 'center' as const },
    { title: 'Tên Khách Hàng', dataIndex: 'name', key: 'name' },
  ];

  return (
    <div>
      <Space style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={3} style={{ margin: 0 }}>Quản lý Khách hàng</Title>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => setIsModalOpen(true)}>
          Thêm Khách Hàng
        </Button>
      </Space>

      <Table 
        dataSource={customers} 
        columns={columns} 
        rowKey="id" 
        loading={loading}
        bordered
      />

      {/* Popup Form Thêm Khách hàng */}
      <Modal
        title="Thêm Khách Hàng Mới"
        open={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        footer={null}
      >
        <Form form={form} layout="vertical" onFinish={handleFinish}>
          <Form.Item 
            name="name" 
            label="Tên Khách Hàng" 
            rules={[{ required: true, message: 'Vui lòng nhập tên khách hàng!' }]}
          >
            <Input placeholder="Nhập tên khách hàng..." />
          </Form.Item>
          <Form.Item style={{ textAlign: 'right', marginBottom: 0 }}>
            <Button onClick={() => setIsModalOpen(false)} style={{ marginRight: 8 }}>Hủy</Button>
            <Button type="primary" htmlType="submit">Lưu lại</Button>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default Customers;