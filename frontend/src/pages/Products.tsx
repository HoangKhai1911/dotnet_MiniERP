import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Modal, Form, Input, InputNumber, message, Tag } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import api from '../api';

const Products: React.FC = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form] = Form.useForm();

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const res = await api.get('v1/Products');
      setProducts(res.data);
    } catch (err) { message.error("Không thể tải danh sách sản phẩm"); }
    setLoading(false);
  };

  useEffect(() => { fetchProducts(); }, []);

  const handleAddProduct = async (values: any) => {
    try {
      await api.post('/Products', values);
      message.success("Thêm sản phẩm thành công");
      setIsModalOpen(false);
      form.resetFields();
      fetchProducts();
    } catch (err) { message.error("Lỗi khi thêm sản phẩm"); }
  };

  const columns = [
    { title: 'SKU', dataIndex: 'sku', key: 'sku', render: (text: string) => <Tag color="blue">{text}</Tag> },
    { title: 'Tên sản phẩm', dataIndex: 'name', key: 'name' },
    { title: 'Giá', dataIndex: 'price', key: 'price', render: (val: number) => `${val.toLocaleString()} ₫` },
    { title: 'Danh mục ID', dataIndex: 'categoryId', key: 'categoryId' },
    { 
      title: 'Thao tác', key: 'action', 
      render: (_: any, record: any) => (
        <Space>
          <Button icon={<EditOutlined />} />
          <Button danger icon={<DeleteOutlined />} />
        </Space>
      ) 
    },
  ];

  return (
    <div>
      <div style={{ marginBottom: 16, display: 'flex', justifyContent: 'space-between' }}>
        <h2>Quản lý Sản phẩm</h2>
        <Button type="primary" icon={<PlusOutlined />} onClick={() => setIsModalOpen(true)}>Thêm Sản phẩm</Button>
      </div>
      <Table columns={columns} dataSource={products} rowKey="id" loading={loading} />

      <Modal title="Thêm Sản phẩm Mới" open={isModalOpen} onCancel={() => setIsModalOpen(false)} onOk={() => form.submit()}>
        <Form form={form} onFinish={handleAddProduct} layout="vertical">
          <Form.Item name="sku" label="Mã SKU" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item name="name" label="Tên sản phẩm" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item name="price" label="Giá bán" rules={[{ required: true }]}><InputNumber style={{ width: '100%' }} /></Form.Item>
          <Form.Item name="categoryId" label="ID Danh mục" rules={[{ required: true }]}><InputNumber style={{ width: '100%' }} /></Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default Products;