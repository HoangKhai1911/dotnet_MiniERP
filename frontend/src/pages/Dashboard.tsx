import React, { useEffect, useState } from 'react';
import { Row, Col, Card, Statistic, Typography, message } from 'antd';
import { 
  AppstoreOutlined, 
  ShoppingCartOutlined, 
  UserOutlined, 
  DollarOutlined 
} from '@ant-design/icons';
import api from '../api'; // Import api đã cấu hình token

const { Title } = Typography;

const Dashboard: React.FC = () => {
  const [stats, setStats] = useState({
    products: 0,
    inventory: 0,
  });

  useEffect(() => {
    // Gọi thử API để lấy số lượng sản phẩm (Ví dụ)
    const fetchStats = async () => {
      try {
        const prodRes = await api.get('/Products');
        setStats(prev => ({ ...prev, products: prodRes.data.length }));
      } catch (error) {
        console.error("Lỗi khi tải dữ liệu Dashboard", error);
      }
    };
    
    fetchStats();
  }, []);

  return (
    <div>
      <Title level={3}>Tổng quan Hệ thống</Title>
      <Row gutter={[16, 16]}>
        <Col xs={24} sm={12} md={6}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.05)' }}>
            <Statistic 
              title="Tổng Sản Phẩm" 
              value={stats.products} 
              prefix={<AppstoreOutlined style={{ color: '#1890ff' }} />} 
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.05)' }}>
            <Statistic 
              title="Đơn Hàng (Tháng này)" 
              value={15} 
              prefix={<ShoppingCartOutlined style={{ color: '#52c41a' }} />} 
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.05)' }}>
            <Statistic 
              title="Khách Hàng" 
              value={42} 
              prefix={<UserOutlined style={{ color: '#faad14' }} />} 
            />
          </Card>
        </Col>
        <Col xs={24} sm={12} md={6}>
          <Card bordered={false} style={{ boxShadow: '0 2px 8px rgba(0,0,0,0.05)' }}>
            <Statistic 
              title="Doanh Thu" 
              value={12500000} 
              prefix={<DollarOutlined style={{ color: '#f5222d' }} />} 
              suffix="₫" 
            />
          </Card>
        </Col>
      </Row>
    </div>
  );
};

export default Dashboard;