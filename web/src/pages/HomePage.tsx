import React from 'react';
import TodoTable from "../features/todo/TodoTable";

export const HomePage: React.FC = () => {
    return (
        <div className="flex flex-col justify-center scale-3.5 mt-10" style={{ minHeight: 'calc(78vh)' }}>
            <div className="mx-auto">
                <TodoTable />
            </div>
        </div>
    );
};

export default HomePage;
