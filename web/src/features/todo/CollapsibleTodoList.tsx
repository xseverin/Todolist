import React from 'react';
import TodoRow from './TodoRow';
import { Todo } from './Todo';

interface CollapsibleTodoListProps {
    todos: Todo[];
    collapsedTodoIds: Set<string>;
    toggleCollapse: (id: string) => void;
    fetchTodos: () => void;
    disableActions?: boolean;
    disableEdit?: boolean;
    deep?: number;
}

const CollapsibleTodoList: React.FC<CollapsibleTodoListProps> = ({
                                                                     todos,
                                                                     collapsedTodoIds,
                                                                     toggleCollapse,
                                                                     fetchTodos,
                                                                     disableActions,
                                                                     disableEdit,
                                                                     deep = 0, // Default deep value if not provided
                                                                 }) => {
    return (
        <>
            {todos.map((todo) => (
                <React.Fragment key={todo.Id}>
                    <TodoRow
                        todo={todo}
                        fetchTodos={fetchTodos}
                        collapsedTodoIds={collapsedTodoIds}
                        toggleCollapse={toggleCollapse}
                        disableActions={disableActions}
                        disableEdit={disableEdit}
                        deep={deep} // Pass the deep value here
                    />
                    {todo.Children && todo.Children.length > 0 && (
                        <div
                            className={`hierarchy-line ${todo.Deep > 1 ? '' : ''} ${collapsedTodoIds.has(todo.Id) ? 'collapsed' : 'expanded'}`}
                            
                        >

                            <div className="pl-8">
                            <div className={collapsedTodoIds.has(todo.Id) ? 'collapsed-content' : 'expanded-content'}>
                                <CollapsibleTodoList
                                    todos={todo.Children}
                                    collapsedTodoIds={collapsedTodoIds}
                                    toggleCollapse={toggleCollapse}
                                    fetchTodos={fetchTodos}
                                    disableActions={disableActions}
                                    disableEdit={disableEdit}
                                    deep={deep + 1} // Increment the deep level for children
                                />
                                {!disableActions && (
                                    <div>
                                        <TodoRow fetchTodos={fetchTodos} parentId={todo.Id} />
                                    </div>
                                )}
                            </div>
                        </div>
                        </div>
                    )}
                </React.Fragment>
            ))}
        </>
    );
};

export default CollapsibleTodoList;
