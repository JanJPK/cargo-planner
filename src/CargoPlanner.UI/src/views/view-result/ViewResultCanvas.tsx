import React, { FC, useState, useRef, useEffect } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { getResult } from '../../services/ResultService';
import * as three from 'three';
import { render } from '@testing-library/react';
import {
  Canvas,
  extend,
  useFrame,
  useThree,
  ReactThreeFiber,
} from 'react-three-fiber';
import { Container, Card, ListGroup, Row, Col } from 'react-bootstrap';
import { Result, Truck, Item } from '../../models/Result';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

extend({ OrbitControls });
declare global {
  namespace JSX {
    interface IntrinsicElements {
      orbitControls: ReactThreeFiber.Object3DNode<
        OrbitControls,
        typeof OrbitControls
      >;
    }
  }
}

interface Props {
  truck: Truck;
  boxClickHandler: (item: Item) => void;
}

const CameraControls = () => {
  const {
    camera,
    gl: { domElement },
  } = useThree();
  const controls = useRef<OrbitControls>();
  useFrame((state) => controls?.current?.update());
  return <orbitControls ref={controls} args={[camera, domElement]} />;
};

const ViewResultCanvas: FC<Props> = ({ truck, boxClickHandler }) => {
  function TruckBottom() {
    const mesh = useRef();
    return (
      <mesh
        ref={mesh}
        scale={[1, 1, 1]}
        position={[truck.mesh.x, -1.0, truck.mesh.z]}
      >
        <boxBufferGeometry
          attach="geometry"
          args={[truck.width, 2, truck.depth]}
        />
        <meshStandardMaterial attach="material" color={truck.mesh.color} />
      </mesh>
    );
  }

  function TruckTop() {
    const mesh = useRef();
    return (
      <mesh
        ref={mesh}
        scale={[1, 1, 1]}
        position={[truck.mesh.x, truck.mesh.y, truck.mesh.z]}
      >
        <boxBufferGeometry
          attach="geometry"
          args={[truck.width, truck.height, truck.depth]}
        />
        <meshStandardMaterial
          attach="material"
          color={truck.mesh.color}
          opacity={truck.mesh.opacity}
          transparent={true}
        />
      </mesh>
    );
  }

  function ItemMesh(item: Item, key: number) {
    const mesh = useRef();
    const [hovered, setHover] = useState(false);

    return (
      <mesh
        key={key}
        ref={mesh}
        scale={[1, 1, 1]}
        onClick={(e) => boxClickHandler(item)}
        onPointerOver={(e) => setHover(true)}
        onPointerOut={(e) => setHover(false)}
        position={[item.mesh.x, item.mesh.y, item.mesh.z]}
      >
        <boxBufferGeometry
          attach="geometry"
          args={[item.width, item.height, item.depth]}
        />
        <meshStandardMaterial
          attach="material"
          color={hovered ? 'hotpink' : item.mesh.color}
          opacity={item.mesh.opacity}
        />
      </mesh>
    );
  }

  return (
    <Container fluid>
      <Row>
        <Col>
          <Card style={{ margin: 10 }}>
            <Card.Body>
              <Card.Title>3D view</Card.Title>
              <Canvas style={{ height: 800 }}>
                <CameraControls />
                <ambientLight />
                {truck.items.map((item, i) => {
                  return ItemMesh(item, i);
                })}
                <TruckTop />
                <TruckBottom />
              </Canvas>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ViewResultCanvas;
