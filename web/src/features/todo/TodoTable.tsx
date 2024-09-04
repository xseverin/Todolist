import React, { useState, useEffect } from 'react';
import CollapsibleTodoList from './CollapsibleTodoList';
import TodoRow from "./TodoRow";
import { Todo } from './Todo';
import { FetchTodosApi } from './todoAPI';

interface TodoTableProps {
    disableActions?: boolean;
}

const TodoTable: React.FC<TodoTableProps> = ({ disableActions = false }) => {
    const [todos, setTodos] = useState<Todo[]>([]);
    const [collapsedTodoIds, setCollapsedTodoIds] = useState<Set<string>>(new Set<string>());

    const fetchTodos = async () => {
        try {
            const fetchedTodos = await FetchTodosApi();
            setTodos(fetchedTodos);
        } catch (error) {
            console.error('Failed to fetch todos:', error);
        }
    };

    useEffect(() => {
        fetchTodos();
    }, []);

    const toggleCollapse = (id: string) => {
        setCollapsedTodoIds(prev => {
            const newCollapsedTodoIds = new Set(prev);
            if (newCollapsedTodoIds.has(id)) {
                newCollapsedTodoIds.delete(id);
            } else {
                newCollapsedTodoIds.add(id);
            }
            return newCollapsedTodoIds;
        });
    };

    // Determine if there are no todos
    const showPlaceholder = todos.length === 0;

    return (
        <div className="w-full ease-out hover:ease-in">
            <CollapsibleTodoList
                todos={todos}
                collapsedTodoIds={collapsedTodoIds}
                toggleCollapse={toggleCollapse}
                fetchTodos={fetchTodos}
                disableActions={disableActions}
            />
            {!disableActions && (
                <TodoRow
                    fetchTodos={fetchTodos}
                    showPlaceholder={showPlaceholder} // Pass showPlaceholder prop
                />
            )}
        </div>
    );
};

export default TodoTable;
